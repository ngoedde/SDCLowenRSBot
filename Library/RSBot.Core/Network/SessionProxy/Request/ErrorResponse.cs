using RSBot.Core.Network.Protocol;

namespace RSBot.Core.Network.SessionProxy.Request;

public class ErrorResponse : SessionProxyObject

{
    public const string Name = "app/error";
    public string Message;
    public int Code;

    public ErrorResponse(string operation = Name) : base(operation)
    {
    }

    public override byte[] ToArray()
    {
        base.ToArray();

        _writer.Write(Code);
        _writer.Write(Message);

        return _writer.GetBytes();
    }

    public override void Read(PacketReader reader)
    {
        Code = reader.ReadInt32();
        Message = reader.ReadString();
    }
}