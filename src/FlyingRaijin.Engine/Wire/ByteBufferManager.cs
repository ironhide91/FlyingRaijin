using System;
using System.Buffers;

namespace FlyingRaijin.Engine.Wire
{
    internal abstract class ByteBufferManager
    {
        public ByteBufferManager()
        {

        }

        private IMemoryOwner<byte> buffer;

        internal ReadOnlyMemory<byte> Buffer { get { return buffer.Memory; } }

        internal void InitializeBuffer(int bufferSize)
        {
            buffer = ByteMemoryPool.Rent(bufferSize);
        }

        internal void CopyFrom(ReadOnlySpan<byte> source)
        {
            source.CopyTo(buffer.Memory.Span);
        }

        internal void ReleaseBuffer()
        {
            buffer.Dispose();
        }
    }
}