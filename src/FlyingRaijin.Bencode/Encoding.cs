using System.Text;

namespace FlyingRaijin.Bencode
{
    public class BencodeEncoding
    {
        public static readonly Encoding CurrentEncoding = new UTF8Encoding(false, false);
    }
}