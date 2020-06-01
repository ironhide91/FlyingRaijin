using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
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
                    if (key is BString)
                    {
                        (parent as BDictionary).Value.Add((BString)key, bString);
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

        private static ErrorType ParseSingleString(ReadOnlySpan<byte> bytes, ref int index, IBObject parent, out IBObject parsedValue)
        {
            parsedValue = null;

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int start = -1;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            if (bytes[(index + 1)] == colon)
            {
                var stringLength = bytes[index].ToInt();

                ++index;

                ++index;

                start = index;

                index += (stringLength - 1);

                parsedValue = new BString(parent, bytes.Slice(start, stringLength).ToArray());

                return ErrorType.None;
            }

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

                var stringLength = int.Parse(buffer);

                ArrayPool<char>.Shared.Return(buffer, true);

                var strStart = ++index;

                index += (stringLength - 1);

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