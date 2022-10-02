using System.IO;

namespace RSBot.Core.Network.Protocol
{
    public class PacketReader : BinaryReader
    {
        private byte[] _buffer;

        public PacketReader(byte[] input)
            : base(new MemoryStream(input, false))
        {
            _buffer = input;
        }

        public PacketReader(byte[] input, int index, int count)
            : base(new MemoryStream(input, index, count, false))
        {
            _buffer = input;
        }
    }
}