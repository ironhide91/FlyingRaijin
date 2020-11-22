using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class Parser
    {
        private static readonly byte[] EmptyStringBytes = Enumerable.Empty<byte>().ToArray();

        private const string InfoPiecesKey = "pieces";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ErrorType ParseString(
            ReadOnlySpan<byte> bytes,
            IBObject parent,
            ref int index,            
            ref bool expectingKey,
            ref IBObject key)
        {
            bool isPiecesKey = ((key != null) && ((BString)key).StringValue.Equals(InfoPiecesKey));

            var error = ParseSingleString(bytes, parent, isPiecesKey, ref index, out IBObject bString);

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
        private static ErrorType ParseSingleString(
            ReadOnlySpan<byte> bytes,
            IBObject parent,
            bool isPieces,
            ref int index,            
            out IBObject parsedValue)
        {
            parsedValue = null;

            // Minimum length for String should be 2
            if (bytes.Length < 2)
                return ErrorType.StringInvalid;

            ErrorType error = ErrorType.None;

            // string Length <= 9
            if (bytes[(index + 1)] == colon)
            {                
                var stringLength = bytes[index].ToInt();

                ++index;
                ++index;

                if (stringLength <= 9)
                {
                    // empty string
                    if ((stringLength == 0) && (bytes.Length == 2))
                    {
                        parsedValue = new BString(parent, EmptyStringBytes);
                        return error;
                    }

                    // non empty string
                    ParseSingleStringLengthLtOrEqualTo9(bytes, parent, stringLength, ref index, out parsedValue, out error);
                    return error;
                }                
            }

            // string Length > 9
            if (isPieces)
            {
                ParseSingleStringLengthGt9Pieces(bytes, parent, ref index, out parsedValue, out error);
                return error;
            }

            ParseSingleStringLengthGt9NonPieces(bytes, parent, ref index, out parsedValue, out error);
            return error;          
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseSingleStringLengthLtOrEqualTo9(
            ReadOnlySpan<byte> bytes,
            IBObject parent,
            int stringLength,
            ref int index,
            out IBObject parsedValue,
            out ErrorType error)
        {
            parsedValue = null;

            error = ErrorType.None;
            
            if ((stringLength == 1) && (bytes.Length == 3))
            {
                parsedValue = new BString(parent, bytes.Slice(2, 1).ToArray());                    
                return;
            }            

            if ((index + stringLength - 1) > (bytes.Length - 1))
            {
                error = ErrorType.StringLessCharsThanSpecified;
                return;
            }

            parsedValue = new BString(parent, bytes.Slice(index, stringLength).ToArray());

            index += (stringLength - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseSingleStringLengthGt9Pieces(
            ReadOnlySpan<byte> bytes,
            IBObject parent,
            ref int index,
            out IBObject parsedValue,
            out ErrorType error)
        {
            parsedValue = null;

            error = ErrorType.None;

            var start = index;

            while (PositiveIntegerBytes.Contains(bytes[++index]))
            {

            }

            if (bytes[index] != colon)
                error = ErrorType.StringInvalid;

            int end = (index - 1);

            var length = (end - start + 1);

            var strLenghtBytes = bytes.Slice(start, length);

            var buffer = ArrayPool<char>.Shared.Rent(length);

            strLenghtBytes.ToChars(buffer, strLenghtBytes.Length);

            if (!int.TryParse(buffer, out int stringLength))
            {
                ArrayPool<char>.Shared.Return(buffer, true);
                error = ErrorType.StringInvalidStringLength;
                return;
            }

            ArrayPool<char>.Shared.Return(buffer, true);

            var strStart = ++index;

            index += (stringLength - 1);

            if (index > (bytes.Length - 1))
            {
                error = ErrorType.StringLessCharsThanSpecified;
                return;
            }

            parsedValue = new BString(parent, bytes.Slice(strStart, stringLength).ToArray());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseSingleStringLengthGt9NonPieces(
            ReadOnlySpan<byte> bytes,
            IBObject parent,
            ref int index,
            out IBObject parsedValue,
            out ErrorType error)
        {
            parsedValue = null;

            error = ErrorType.None;

            var start = index;

            while (PositiveIntegerBytes.Contains(bytes[++index]))
            {

            }

            if (bytes[index] != colon)
            {
                error = ErrorType.StringInvalid;
                return;
            }

            int end = (index - 1);
            var length = (end - start + 1);
            var strLenghtBytes = bytes.Slice(start, length);

            var buffer = ArrayPool<char>.Shared.Rent(length);

            strLenghtBytes.ToChars(buffer, strLenghtBytes.Length);

            if (!int.TryParse(buffer, out int stringLength))
            {
                ArrayPool<char>.Shared.Return(buffer, true);
                error = ErrorType.StringInvalidStringLength;
                return;
            }

            ArrayPool<char>.Shared.Return(buffer, true);

            if (stringLength >= int.MaxValue)
            {
                error = ErrorType.StringInvalidStringLength;
                return;
            }

            if (stringLength > bytes.Length)
            {
                error = ErrorType.StringLessCharsThanSpecified;
                return;
            }

            var strStart = ++index;            

            buffer = ArrayPool<char>.Shared.Rent(Encoding.UTF8.GetMaxCharCount(bytes.Length - index));
            Span<char> charSpan = buffer;
            Encoding.UTF8.GetChars(bytes.Slice(index), charSpan);
            var value = charSpan.Slice(0, stringLength);            

            var bytesRead = Encoding.UTF8.GetByteCount(value);
            var byteBuffer = ArrayPool<byte>.Shared.Rent(bytesRead);
            Span<byte> byteSpan = byteBuffer;
            Encoding.UTF8.GetBytes(value, byteSpan);

            index += (bytesRead - 1);

            parsedValue = new BString(parent, byteSpan.Slice(0, bytesRead).ToArray());

            ArrayPool<char>.Shared.Return(buffer, true);
            ArrayPool<byte>.Shared.Return(byteBuffer, true);
        }        

        private static int ToInt(this byte b)
        {
            return (b - '0');
        }
    }
}