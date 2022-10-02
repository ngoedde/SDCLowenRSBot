using System.IO;

namespace RSBot.Core.Network.Protocol
{
    public class PacketWriter : BinaryWriter
    {
        private MemoryStream _memoryStream;

        public PacketWriter()
        {
            _memoryStream = new MemoryStream();
            OutStream = _memoryStream;
        }

        public byte[] GetBytes()
        {
            return _memoryStream.ToArray();
        }
    }
}