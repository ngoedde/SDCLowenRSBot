using System;
using RSBot.Core.Network.Protocol;

namespace RSBot.Core.Network.SessionProxy.Request;

public class CreateSessionResponse : SessionProxyObject
{
    public const string Name = "app/session-created";

    public Guid? SessionId;

    public string ClientRedirectIp;

    public ushort ClientRedirectPort;

    public string ProxyIp;
    
    public ushort ProxyPort;

    public CreateSessionResponse(string operation = Name) : base(operation)
    {
    }
    public override byte[] ToArray()
    {
        base.ToArray();

        _writer.Write(SessionId.ToString());
        _writer.Write(ClientRedirectIp);
        _writer.Write(ClientRedirectPort);
        _writer.Write(ProxyIp);
        _writer.Write(ProxyPort);

        return _writer.GetBytes();
    }

    public override void Read(PacketReader reader)
    {
        base.Read(reader);

        SessionId = Guid.Parse(reader.ReadString());
        ClientRedirectIp = reader.ReadString();
        ClientRedirectPort = reader.ReadUInt16();
        ProxyIp = reader.ReadString();
        ProxyPort = reader.ReadUInt16();
    }
}