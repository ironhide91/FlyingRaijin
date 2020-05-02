using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class ParserContext : IDisposable
    {
        public ParserContext(Stream stream)
        {
              Stream = stream;
              Reader = new StreamReader(Stream, CurrentEncoding);
        }

        public ParserContext(string bencodeString)
        {
              Stream = new MemoryStream(CurrentEncoding.GetBytes(bencodeString));
              Reader = new StreamReader(Stream, CurrentEncoding);
        }

        private static readonly Encoding CurrentEncoding = new UTF8Encoding(false, false);

        private readonly Stream Stream;

        private readonly StreamReader Reader;

        public char LookAheadChar { get; private set; }

        public void Match(char charToMatch)
        {
            CheckEndOfStream();

            var value = Reader.Peek();

            if ((char)value == charToMatch)
            {
                Reader.Read();
                LookAheadChar = (char)Reader.Peek();
                return;
            }

            throw ParsingException.Create("Invalid Bencode.");
        }

        public bool IsMatch(char charToMatch)
        {
            CheckEndOfStream();

            var value = (char)Reader.Peek();

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

            var value = (char)Reader.Peek();

            if (charsToMatch.Contains(value))
            {
                LookAheadChar = value;
                return true;
            }

            return false;
        }

        public void Advance(int numBytes, byte[] buffer)
        {
            var result = Reader.BaseStream.Read(buffer, 0, numBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckEndOfStream()
        {
            if (Reader.EndOfStream)
                throw ParsingException.Create("Reached End Of Stream.");
        }

        public void Dispose()
        {
            if (Stream != null)
            {
                Stream.Dispose();
                Reader.Dispose();
            }
        }
    }
}