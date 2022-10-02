using RSBot.Core.Network.Protocol;
using RSBot.Core.Network.SessionProxy.Request;
using RSBot.Proxy.Network.Intercom;
using System.Net;
using System.Net.Sockets;

namespace RSBot.Proxy.Network;

internal class Server : IDisposable
{
    private const int MaxPacketSize = 1024;

    private Socket _socket;

    public IPEndPoint Binding { get; init; }

    public Dictionary<int, Client> ConnectedBots { get; private set; }

    public Server(IPEndPoint binding)
    {
        Binding = binding;
        ConnectedBots = new Dictionary<int, Client>(5);
    }

    public async Task Listen()
    {
        _socket = new Socket(Binding.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(Binding);
        _socket.Listen(5);

        Console.WriteLine($"Listening for RSBot connections on [{Binding.Address}:{Binding.Port}]");

        try
        {
            var handler = await _socket.AcceptAsync();

            ConnectedBots.Add(handler.Handle.ToInt32(), new Client(handler));

            Console.WriteLine($"New connection [{handler.AddressFamily}]");

            while (true)
            {
                try
                {
                    var buffer = new byte[MaxPacketSize];
                    await handler.ReceiveAsync(buffer, SocketFlags.None);

                    using var packetReader = new PacketReader(buffer);

                    await HandleRequest(packetReader, handler.Handle.ToInt32());
                }
                catch (SocketException se)
                {
                    if (se.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        ConnectedBots.Remove(handler.Handle.ToInt32());

                        Console.WriteLine($"Closed session to bot {handler.RemoteEndPoint}");
                    }

                    handler.Dispose();
                    await _socket.AcceptAsync();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
       
    }

    public async Task<byte[]> HandleRequest(PacketReader packetReader, int handle)
    {
        var operation = packetReader.ReadString();

        switch (operation)
        {
            case CreateSessionObject.Name:
                var session = new CreateSessionObject(operation);
                session.Read(packetReader);
                await AddSessionAsync(session, handle);

                break;
        }

        return new byte[1024];
    }

    private async Task AddSessionAsync(CreateSessionObject createSessionObject, int handle)
    {
        var session = new Session()
        {
            GatewayIp = createSessionObject.GatewayIp,
            GatewayPort = createSessionObject.GatewayPort,
            Id = Guid.NewGuid(),
            Profile = createSessionObject.Profile
        };

        if (!ConnectedBots.TryGetValue(handle, out var client))
        {
            Console.WriteLine("Invalid session @object: Handle could not be found!");

            return;
        }

        client.Session = session;

        try
        {
            client.CreateProxy();

            var response = new CreateSessionResponse
            {
                SessionId = session.Id,
                ClientRedirectIp = "127.0.0.1",
                ClientRedirectPort = client.Proxy.Port,
                ProxyIp =  client.Proxy.Server.IP,
                ProxyPort =  client.Proxy.Server.Port
            };

            await client.SendAsync(response);
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not create proxy: {e.Message}");
        }

        Console.WriteLine($"New session created! {session.Id} ({session.GatewayIp}:{session.GatewayPort}");
    }

    public void Dispose()
    {
        if (_socket != null)
            _socket.Close();
    }
}