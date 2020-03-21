using System.Text;

namespace FlyingRaijin.Client.Bencode
{
    public interface IBencodeWriter
    {
        string Write(Encoding encoding, string path);
    }
}