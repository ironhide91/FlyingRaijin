using System;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<byte> SliceExclude(this ReadOnlySpan<byte> bytes, int start, int end)
        {
            if (start == end)
                return bytes.Slice(start, 1);

            return bytes.Slice((start + 1), (end - start - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ToChars(this ReadOnlySpan<byte> bytes, char[] chars, int length)
        {
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)bytes[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IndexWithinBound(this ReadOnlySpan<byte> bytes, int nextIndex)
        {
            if (bytes.IsEmpty)
                return false;

            if (bytes.Length == (nextIndex + 1))
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasError(this ErrorType error)
        {
            return (error != ErrorType.None);
        }
    }
}