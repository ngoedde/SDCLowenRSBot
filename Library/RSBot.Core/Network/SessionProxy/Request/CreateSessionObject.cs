namespace RSBot.Core.Network.SessionProxy.Request;

public class CreateSessionObject : SessionProxyObject
{
    public const string Name = "app/create-session";

    public string GatewayIp;
    public ushort GatewayPort;
    public string Profile;

    public CreateSessionObject(string operation = Name) : base(operation)
    {
    }

    public override byte[] ToArray()
    {
        base.ToArray();

        _writer.Write(GatewayIp);
        _writer.Write(GatewayPort);
        _writer.Write(Profile);

        return _writer.GetBytes();
    }
}