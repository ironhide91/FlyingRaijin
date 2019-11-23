using FlyingRaijin.Bencode.Exceptions;
using System;
using System.IO;
using System.Text;

namespace FlyingRaijin.Bencode.Parser
{
    public sealed class ParseContext : IDisposable
    {
        public ParseContext(Encoding encoding, Stream stream)
        {
            Encoding = encoding;
            Stream = stream;
            Reader = new BinaryReader(Stream, Encoding);
        }

        public readonly Encoding Encoding;

        private readonly Stream Stream;

        private readonly BinaryReader Reader;

        public byte LookAheadByte => HasTokens();

        public byte HasTokens()
        {
            var nextByte = Reader.ReadByte();
            Reader.BaseStream.Position = Reader.BaseStream.Position - 1;

            return nextByte;
        }

        public void Match(byte byteToMatch)
        {
            if (LookAheadByte == byteToMatch)
            {
                Reader.ReadByte();
                return;
            }

            throw ParsingException.Create(Reader.BaseStream.Position+1);
        }

        public void Dispose()
        {
            if (Stream != null)
            {
                Stream.Dispose();
            }
        }
    }
}