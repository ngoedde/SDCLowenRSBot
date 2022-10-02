using System.Diagnostics;
using System.IO;
using RSBot.Core.Network.Protocol;

namespace RSBot.Core.Network.SessionProxy.Request;

public abstract class SessionProxyObject
{
    protected PacketReader _reader;
    protected PacketWriter _writer;

    public virtual bool CanRead => _reader != null && _writer != null;

    public virtual bool CanWrite => _writer != null && _writer.BaseStream.CanWrite;

    public string Operation { get; }

    public SessionProxyObject(string operation)
    {
        Operation = operation;
    }

    public SessionProxyObject(string operation, PacketReader reader)
    {
        Operation = operation;
        
        _reader = reader;
    }

    public virtual void Read(PacketReader reader)
    {
        _reader = reader;
    }

    public virtual byte[] ToArray()
    {
        _writer = new PacketWriter();

        _writer.Write(Operation);

        return _writer.GetBytes();
    }
}