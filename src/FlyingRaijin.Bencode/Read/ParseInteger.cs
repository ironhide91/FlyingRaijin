using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        private static ErrorType ParseInteger(ReadOnlySpan<byte> bytes, ref int index, IBObject parent, IBObject key)
        {
            var error = ParseSingleInteger(bytes, ref index, parent, out IBObject bInteger);

            if (error.HasError())
                return error;

            if (parent is BDictionary)
            {
                if (key is BString)
                {
                    (parent as BDictionary).Value.Add((BString)key, bInteger);
                    return ErrorType.None;
                }

                return ErrorType.KeyShouldBeString;
            }

            if (parent is BList)
            {
                (parent as BList).Value.Add(bInteger);
                return ErrorType.None;
            }

            return ErrorType.Unknown;
        }

        private static ErrorType ParseSingleInteger(ReadOnlySpan<byte> bytes, ref int index, IBObject parent, out IBObject parsedValue)
        {
            parsedValue = null;

            if ((bytes[++index] == zero) & ((bytes[++index] == end)))
            {
                parsedValue = new BInteger(parent, 0L);
                return ErrorType.None;
            }

            --index;
            --index;

            var start = index;

            if (NonZeroIntegerBytes.Contains(bytes[++index]))
            {
                while (PositiveIntegerBytes.Contains(bytes[++index]))
                {

                }

                if (bytes[index] == end)
                {
                    var intBytes = bytes.SliceExclude(start, index);

                    var buffer = ArrayPool<char>.Shared.Rent(intBytes.Length);

                    intBytes.ToChars(buffer, intBytes.Length);

                    var value = long.Parse(buffer);

                    ArrayPool<char>.Shared.Return(buffer, true);

                    parsedValue = new BInteger(parent, value);

                    return ErrorType.None;
                }

                return ErrorType.IntegerMustEndWithE;
            }

            return ErrorType.IntegerTrailingZeroAfterZero;
        }
    }
}
