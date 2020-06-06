using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        private static int Int64MinValueCharLength = long.MinValue.ToString().Length;
        private static int Int64MaxValueCharLength = long.MaxValue.ToString().Length;

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

            // Minimum length for Integer should be 3
            if (bytes.Length < 3)
                return ErrorType.IntegerInvalid;

            // Zero Value
            if ((bytes[++index] == zero) & ((bytes[++index] == end)))
            {
                parsedValue = new BInteger(parent, 0L);
                return ErrorType.None;
            }

            --index;
            --index;

            // "i-e" is invalid
            if ((bytes[++index] == minus) & ((bytes[++index] == end)))
            {
                parsedValue = new BInteger(parent, 0L);
                return ErrorType.IntegerInvalid;
            }

            --index;
            --index;

            // "i-0*" is invalid
            if ((bytes[++index] == minus) & ((bytes[++index] == zero)))
            {
                parsedValue = new BInteger(parent, 0L);
                return ErrorType.IntegerInvalid;
            }

            --index;
            --index;

            var start = index;

            if (NonZeroIntegerBytes.Contains(bytes[++index]))
            {
                while (bytes.IndexWithinBound(index) && PositiveIntegerBytes.Contains(bytes[++index]))
                {

                }

                if (bytes[index] == end)
                {
                    var intBytes = bytes.SliceExclude(start, index);

                    if (intBytes[0] == minus && (intBytes.Length > Int64MinValueCharLength))
                    {
                        return ErrorType.IntegerOutOfInt64Range;
                    }

                    if (intBytes[0] != minus && intBytes.Length > Int64MaxValueCharLength)
                    {
                        return ErrorType.IntegerOutOfInt64Range;
                    }

                    var buffer = ArrayPool<char>.Shared.Rent(intBytes.Length);

                    intBytes.ToChars(buffer, intBytes.Length);

                    long value;

                    if (long.TryParse(buffer, out value))
                    {
                        ArrayPool<char>.Shared.Return(buffer, true);

                        parsedValue = new BInteger(parent, value);

                        return ErrorType.None;
                    }

                    return ErrorType.IntegerOutOfInt64Range;
                }

                return ErrorType.IntegerInvalid;
            }

            return ErrorType.IntegerInvalid;
        }
    }
}