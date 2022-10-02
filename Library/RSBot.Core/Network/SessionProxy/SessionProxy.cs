using RSBot.Core.Network.Protocol;
using RSBot.Core.Network.SessionProxy.Request;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using RSBot.Core.Components;

namespace RSBot.Core.Network.SessionProxy;

public class SessionProxy
{
    public bool IsConnected => _socket is { Connected: true };

    public bool IsAuthenticated { get; private set; }
    public bool IsAuthenticating { get; private set; }

    
    public CreateSessionResponse Session { get; private set; }
    private Guid? SessionId { get; set; }
    private Socket? _socket;
    private byte[]? _buffer;

    public async Task Connect(IPEndPoint endpoint)
    {
        if (IsConnected)
            return;

        IsAuthenticating = true;


        _socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await _socket.ConnectAsync(endpoint);
        }
        catch (Exception ex)
        {
            Log.Warn("[SessionManager] Could not connect to SessionProxy!");
        }
        finally
        {
            IsAuthenticating = false;
        }

    }

    private async Task Listen()
    {
        while (true)
        {
            var bufferSize = await _socket.ReceiveAsync(_buffer, SocketFlags.None);
            _buffer = new byte[bufferSize];

            if (bufferSize == 0)
                break;

            using var reader = new PacketReader(_buffer);
            var operation = reader.ReadString();

            switch (operation)
            {
                case CreateSessionResponse.Name:
                    var sessionObj = new CreateSessionResponse();
                    sessionObj.Read(reader);
                    Session = sessionObj;
                    
                    break;
            }
        }
    }

    public void Disconnect()
    {
        if (!IsConnected)
            return;

        _socket?.Close();
    }

    public void Authenticate()
    {
        if (IsAuthenticated || IsAuthenticating)
            return;

        IsAuthenticating = true;

        var divisionIndex = GlobalConfig.Get<int>("RSBot.DivisionIndex");
        var severIndex = GlobalConfig.Get<int>("RSBot.GatewayIndex");

        var gatewayIP = Game.ReferenceManager.DivisionInfo.Divisions[divisionIndex].GatewayServers[severIndex];
        var gatewayPort = Game.ReferenceManager.GatewayInfo.Port;

        var request = new CreateSessionObject
        {
            GatewayIp = gatewayIP,
            GatewayPort = gatewayPort,
            Profile = ProfileManager.SelectedProfile
        };

        Send(request);
    }

    public async Task Send(SessionProxyObject request)
    {
        if (!IsConnected)
            return;

        await _socket.SendAsync(request.ToArray(), SocketFlags.None, CancellationToken.None);
    }
}