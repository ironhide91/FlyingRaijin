using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FlyingRaijin.Bencode.Parser
{
    //public sealed class ParseContext
    //{
    //    public ParseContext(string bencodedValue)
    //    {
    //        BencodedValue = bencodedValue;
    //        Tokens = Array.AsReadOnly(BencodedValue.ToCharArray());
    //    }

    //    public readonly string BencodedValue;

    //    private readonly ReadOnlyCollection<char> Tokens;

    //    public char LookAheadChar
    //    {
    //        get { return Tokens[LookAheadIndex]; }
    //    }

    //    public int LookAheadIndex { get; private set; }

    //    public void HasTokens()
    //    {
    //        if (LookAheadIndex >= Tokens.Count)
    //        {
    //            throw new Exception("");
    //        }
    //    }

    //    public void Match(char characterToMatch)
    //    {
    //        if (LookAheadChar == characterToMatch)
    //        {
    //            LookAheadIndex++;
    //            return;
    //        }

    //        throw new Exception("Parsing Error.");
    //    }
    //}

    public sealed class ParseContext
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

            throw new Exception("Parsing Error.");
        }
    }
}