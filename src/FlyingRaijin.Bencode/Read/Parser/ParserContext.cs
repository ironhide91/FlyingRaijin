using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
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

        public readonly StreamReader Reader;

        public char LookAheadChar { get; private set; }

        public void Match(char charToMatch)
        {
            if (Reader.EndOfStream)
            {
                throw ParsingException.Create("Reached End Of Stream.");
            }

            var value = Reader.Peek();

            if ((char)value == charToMatch)
            {
                Reader.Read();
                LookAheadChar = (char)Reader.Peek();
                return;
            }

            throw ParsingException.Create("Invalid Encode.");
        }

        public bool IsMatch(char charToMatch)
        {
            return (charToMatch == (char)Reader.Peek());
        }

        public bool IsMatch(HashSet<char> charsToMatch)
        {
            return charsToMatch.Contains((char)Reader.Peek());
        }

        public void Advance(int numBytes, byte[] buffer)
        {
            var result = Reader.BaseStream.Read(buffer, 0, numBytes);
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