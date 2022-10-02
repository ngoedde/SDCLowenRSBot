using System.Net.Sockets;
using System.Windows.Forms;
using RSBot.Core.Network;
using RSBot.Core.Network.SessionProxy.Request;
using RSBot.Proxy.Network.Intercom;

namespace RSBot.Proxy.Network;

internal class Client
{
    public Session? Session { get; set; }
    public Core.Network.Proxy Proxy { get; set; }

    private Socket _socket;

    
    public Client(Socket socket)
    {
        _socket = socket;
    }
    
    public async Task<int> SendAsync(SessionProxyObject response)
    {
        var buffer = new ReadOnlyMemory<byte>(response.ToArray());
        return await _socket.SendAsync(buffer, SocketFlags.None, CancellationToken.None);
    }

    public void CreateProxy()
    {
        if (Session == null)
            throw new Exception("Could not create proxy because there is no active session!");

        Proxy = new Core.Network.Proxy();
        Proxy.Start(NetworkUtilities.GetFreePort(17500, 18000, 5), Session.GatewayIp, Session.GatewayPort);

        Console.WriteLine($"New proxy created: [127.0.0.1:{Proxy.Port}]");
    }
}