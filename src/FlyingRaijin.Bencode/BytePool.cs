using System.Buffers;

namespace FlyingRaijin.Bencode
{
    internal static class BytePool
    {
        internal static readonly ArrayPool<byte> Pool = ArrayPool<byte>.Shared;
    }
}