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
            var numOfBytes = CurrentEncoding.GetByteCount(bencodeString);
            pooledBuffer = arrayPool.Rent(numOfBytes);
            CurrentEncoding.GetBytes(bencodeString.AsSpan(), pooledBuffer.AsSpan());

            Stream = manager.GetStream();
            Stream.Write(pooledBuffer.AsSpan().Slice(0, numOfBytes));
            Stream.Flush();
            Stream.Position = 0;

            //Stream = new MemoryStream(CurrentEncoding.GetBytes(bencodeString));
            TryPeek();
        }

        private static readonly Encoding CurrentEncoding = new UTF8Encoding(false, false);

        private readonly Stream Stream;

        private byte[] pooledBuffer = null;

        private byte[] buffer = new byte[1];

        public char LookAheadChar { get; private set; }

        public void Match(char charToMatch)
        {
            if (LookAheadChar == charToMatch)
            {
                var result = Stream.Read(buffer, 0, 1);

                if (result == 0)
                    throw ParsingException.Create("Reached End Of Stream.");

                TryPeek();
                return;
            }

            throw ParsingException.Create($"Invalid Bencode.{Environment.NewLine}{Environment.StackTrace}");
        }

        public bool IsMatch(char charToMatch)
        {
            return LookAheadChar == charToMatch;
        }

        public bool IsMatch(HashSet<char> charsToMatch)
        {
            return charsToMatch.Contains(LookAheadChar);
        }

        public void Advance(int numBytes, byte[] buffer)
        {
            var result = Stream.Read(buffer, 0, numBytes);

            if (result == 0)
                throw ParsingException.Create("Reached End Of Stream.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryPeek()
        {
            var result = Stream.Read(buffer, 0, 1);

            if (result == 0)
                return;

            Stream.Position--;

            LookAheadChar = (char)buffer[0];

            return;
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