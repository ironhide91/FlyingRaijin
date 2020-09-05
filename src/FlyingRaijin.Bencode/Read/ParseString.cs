using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class Parser
    {
        private static readonly byte[] EmptyStringBytes = Enumerable.Empty<byte>().ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ErrorType ParseString(ReadOnlySpan<byte> bytes, ref int index, IBObject parent, ref bool expectingKey, ref IBObject key)
        {
            var error = ParseSingleString(bytes, ref index, parent, out IBObject bString);

            if (error.HasError())
                return error;

            if (parent is BDictionary)
            {
                if (expectingKey)
                {
                    key = bString;
                    expectingKey = false;
                    return ErrorType.None;
                }
                else
                {
                    if (key is BString @string)
                    {
                        (parent as BDictionary).Value.Add(@string, bString);
                        expectingKey = true;
                        return ErrorType.None;
                    }

                    return ErrorType.KeyShouldBeString;
                }
            }

            if (parent is BList)
            {
                (parent as BList).Value.Add(bString);
                return ErrorType.None;
            }

            return ErrorType.Unknown;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]        
        private static ErrorType ParseSingleString(ReadOnlySpan<byte> bytes, ref int index, IBObject parent, out IBObject parsedValue)
        {
            parsedValue = null;

            // Minimum length for String should be 2
            if (bytes.Length < 2)
                return ErrorType.StringInvalid;

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int start = -1;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            // string Length <= 9
            if (bytes[(index + 1)] == colon)
            {
                var stringLength = bytes[index].ToInt();

                if ((stringLength == 0) && (bytes.Length == 2))
                {
                    parsedValue = new BString(parent, EmptyStringBytes);
                    return ErrorType.None;
                }

                if ((stringLength == 1) && (bytes.Length == 3))
                {
                    parsedValue = new BString(parent, bytes.Slice(2, 1).ToArray());
                    return ErrorType.None;
                }

                ++index;

                ++index;

                start = index;

                index += (stringLength - 1);

                if (index > (bytes.Length - 1))
                {
                    return ErrorType.StringLessCharsThanSpecified;
                }

                parsedValue = new BString(parent, bytes.Slice(start, stringLength).ToArray());

                return ErrorType.None;
            }

            // string Length > 9
            start = index;

            while (PositiveIntegerBytes.Contains(bytes[++index]))
            {

            }

            int end = (index - 1);

            if (bytes[index] == colon)
            {
                var length = (end - start + 1);

                var strLenghtBytes = bytes.Slice(start, length);

                var buffer = ArrayPool<char>.Shared.Rent(length);

                strLenghtBytes.ToChars(buffer, strLenghtBytes.Length);

                if (!int.TryParse(buffer, out int stringLength))
                {
                    ArrayPool<char>.Shared.Return(buffer, true);
                    return ErrorType.StringInvalidStringLength;
                }

                ArrayPool<char>.Shared.Return(buffer, true);

                var strStart = ++index;

                index += (stringLength - 1);

                if (index < 0)
                {
                    return ErrorType.StringInvalidStringLength;
                }

                if (index > (bytes.Length - 1))
                {
                    return ErrorType.StringLessCharsThanSpecified;
                }

                parsedValue = new BString(parent, bytes.Slice(strStart, stringLength).ToArray());

                return ErrorType.None;
            }

            return ErrorType.StringInvalid;
        }

        private static int ToInt(this byte b)
        {
            return (b - '0');
        }
    }
}