using System.Buffers;

namespace FlyingRaijin.Engine
{
    internal static class ByteMemoryPool
    {
        public static IMemoryOwner<byte> Rent(int minBufferSize)
        {
            return MemoryPool<byte>.Shared.Rent(minBufferSize);
        }
    }
}