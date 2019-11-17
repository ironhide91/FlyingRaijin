using FlyingRaijin.Bencode.Parser;
using System.IO;
using System.Text;

namespace FlyingRaijin.Bencode.Test
{
    public static class Helper
    {
        private static Encoding encoding = Encoding.UTF8;

        public static ParseContext CreateParseContext(string value)
        {
            return new ParseContext(encoding, new MemoryStream(encoding.GetBytes(value)));
        }
    }
}
