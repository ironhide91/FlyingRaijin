using System;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        private static ReadOnlySpan<byte> SliceExclude(this ReadOnlySpan<byte> bytes, int start, int end)
        {
            if (start == end)
                return bytes.Slice(start, 1);

            return bytes.Slice((start + 1), (end - start - 1));
        }

        private static void ToChars(this ReadOnlySpan<byte> bytes, char[] chars, int length)
        {
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)bytes[i];
            }
        }

        private static bool HasError(this ErrorType error)
        {
            return (error != ErrorType.None);
        }
    }
}