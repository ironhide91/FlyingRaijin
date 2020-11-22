using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRaijin.Bencode.Read
{
    internal ref struct UnicodeReader
    {
        public UnicodeReader(Encoding _encoding, ReadOnlySpan<byte> _bytes)
        {
                    encoding = _encoding;
                       bytes = _bytes;
                       index = -1;
                   charCount = encoding.GetCharCount(_bytes);
            singleCharBuffer = new char[1];
        }

        private int index;
        private readonly long charCount;
        private readonly Encoding encoding;
        private readonly ReadOnlySpan<byte> bytes;
        private readonly char[] singleCharBuffer;

        public char ReadChar()
        {

            //encoding.GetChars(_bytes, index, 1);
            return '0';
        }

        public int ReadByte()
        {
            return 0;
        }
    }
}
