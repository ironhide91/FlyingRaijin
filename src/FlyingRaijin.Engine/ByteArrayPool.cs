using System.Buffers;

namespace FlyingRaijin.Engine
{
    internal static class ByteArrayPool
    {
        public static byte[] Rent(int minimumLength)
        {
            return ArrayPool<byte>.Shared.Rent(minimumLength);
        }

        public static byte[] Rent(long minimumLength)
        {
            return ArrayPool<byte>.Shared.Rent((int)minimumLength);
        }

        public static void Return(byte[] array, bool clearArray = false)
        {
            ArrayPool<byte>.Shared.Return(array, clearArray);
        }
    }
}