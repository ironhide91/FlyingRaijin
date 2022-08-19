using System.Buffers;

namespace FlyingRaijin.Engine.Messages
{
    internal class FileRead
    {
        internal FileRead()
        {

        }

        private IMemoryOwner<byte> buffer;

        internal Span<byte> Buffer { get { return buffer.Memory.Span; } }

        internal void InitializeBuffer(int bufferSize)
        {
            buffer = ByteMemoryPool.Rent(bufferSize);
        }

        internal void ReleaseBuffer()
        {
            buffer.Dispose();
        }
    }
}