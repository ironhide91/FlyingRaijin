using FlyingRaijin.Bencode.Read.Exceptions;
using Microsoft.IO;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class ParserContext : IDisposable
    {
        private static readonly RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();

        private static readonly ArrayPool<byte> arrayPool = ArrayPool<byte>.Shared;        

        public ParserContext(Stream stream)
        {
            Stream = stream;
        }

        public ParserContext(string bencodeString)
        {
            //var numOfBytes = CurrentEncoding.GetByteCount(bencodeString);
            //pooledBuffer = arrayPool.Rent(numOfBytes);
            //CurrentEncoding.GetBytes(bencodeString.AsSpan(), pooledBuffer.AsSpan());

            //Stream = manager.GetStream();
            //Stream.Write(pooledBuffer.AsSpan().Slice(0, numOfBytes));
            //Stream.Flush();
            //Stream.Position = 0;

            Stream = new MemoryStream(CurrentEncoding.GetBytes(bencodeString));
        }

        private static readonly Encoding CurrentEncoding = new UTF8Encoding(false, false);

        private readonly Stream Stream;

        private byte[] pooledBuffer = null;

        private byte[] buffer = new byte[1];

        public char LookAheadChar { get; private set; }

        public void Match(char charToMatch)
        {
            CheckEndOfStream();

            var value = PeekChar();

            if (value == charToMatch)
            {
                ReadChar();
                LookAheadChar = PeekChar();
                return;
            }

            throw ParsingException.Create($"Invalid Bencode.{Environment.NewLine}{Environment.StackTrace}");
        }

        public bool IsMatch(char charToMatch)
        {
            CheckEndOfStream();

            var value = PeekChar();

            if (value == charToMatch)
            {
                LookAheadChar = value;
                return true;
            }

            return false;
        }

        public bool IsMatch(HashSet<char> charsToMatch)
        {
            CheckEndOfStream();

            var value = PeekChar();

            if (charsToMatch.Contains(value))
            {
                LookAheadChar = value;
                return true;
            }

            return false;
        }

        public void Advance(int numBytes, byte[] buffer)
        {
            Stream.Read(buffer, 0, numBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckEndOfStream()
        {
            if (Stream.Position > Stream.Length)
                throw ParsingException.Create("Reached End Of Stream.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char PeekChar()
        {
            CheckEndOfStream();

            Stream.Read(buffer, 0, 1);

            Stream.Position--;

            return (char)buffer[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char ReadChar()
        {
            CheckEndOfStream();

            var value = (char)Stream.Read(buffer, 0, 1);

            return value;
        }

        public void Dispose()
        {
            if (Stream != null)
            {
                //arrayPool.Return(pooledBuffer, true);
                Stream.Dispose();
            }
        }
    }
}