using System;
using System.Buffers;

namespace FlyingRaijin.Engine
{
    internal abstract class ByteBufferManager
    {
        public ByteBufferManager()
        {

        }

        protected IMemoryOwner<byte> buffer;

        internal ReadOnlyMemory<byte> Buffer { get { return buffer.Memory; } }

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