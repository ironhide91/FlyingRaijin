using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Bencode.Read
{
    internal static partial class BencodeParser
    {
        private static readonly int Int64MinValueCharLength = long.MinValue.ToString().Length;
        private static readonly int Int64MaxValueCharLength = long.MaxValue.ToString().Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ErrorType ParseInteger(ReadOnlySpan<byte> bytes, IBObject parent, ref int index, IBObject key)
        {
            var error = ParseSingleInteger(bytes, parent, ref index, out IBObject bInteger);

            if (error.HasError())
                return error;

            if (parent is BDictionary)
            {
                if (key is BString @string)
                {
                    (parent as BDictionary).Value.Add(@string, bInteger);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ErrorType ParseSingleInteger(ReadOnlySpan<byte> bytes, IBObject parent, ref int index, out IBObject parsedValue)
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

                    Span<char> spanBuffer = buffer;

                    intBytes.ToChars(buffer, intBytes.Length);

                    if (long.TryParse(spanBuffer.Slice(0, intBytes.Length), out long value))
                    {
                        ArrayPool<char>.Shared.Return(buffer, true);

                        parsedValue = new BInteger(parent, value);

                        return ErrorType.None;
                    }

                    ArrayPool<char>.Shared.Return(buffer, true);

                    return ErrorType.IntegerOutOfInt64Range;
                }

                return ErrorType.IntegerInvalid;
            }

            return ErrorType.IntegerInvalid;
        }
    }
}