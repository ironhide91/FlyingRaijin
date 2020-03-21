using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.IO;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class ParseContext : IDisposable
    {
        public ParseContext(Encoding encoding, Stream stream)
        {
            Encoding = encoding;
            Stream = stream;
            Reader = new BinaryReader(Stream, Encoding);
        }

        public ParseContext(Encoding encoding, string bencodeString)
        {
            Encoding = encoding;
            Stream = new MemoryStream(encoding.GetBytes(bencodeString));
            Reader = new BinaryReader(Stream, Encoding);
        }

        public readonly Encoding Encoding;

        private readonly Stream Stream;

        private readonly BinaryReader Reader;

        public long Position => Reader.BaseStream.Position;

        public byte LookAheadByte => HasTokens();

        public byte HasTokens()
        {
            byte b;

            try
            {
                b = Reader.ReadByte();
                Reader.BaseStream.Position = Reader.BaseStream.Position - 1;
            }
            catch (EndOfStreamException)
            {
                throw ParsingException.Create("Reached end of stream.");
            }            

            return b;
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