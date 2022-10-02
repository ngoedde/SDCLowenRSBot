using System.Net;
using RSBot.Proxy.Network;

Server _server;

StartServer();

while (Console.ReadLine() != "exit")
{
    Console.WriteLine("Enter exit to close the server application");
}

async void StartServer(ushort defaultPort = 17000)
{
    var args = Environment.GetCommandLineArgs();

    int.TryParse(args[0], out var port);

    if (port == 0)
        port = defaultPort;

    _server = new Server(new IPEndPoint(IPAddress.Any, port));

    await _server.Listen();
}
