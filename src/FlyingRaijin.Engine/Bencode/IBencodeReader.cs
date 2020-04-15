using FlyingRaijin.Engine.Torrent;
using System.Text;

namespace FlyingRaijin.Engine.Bencode
{
    public interface IBencodeReader
    {
        SingleFileTorrent ReadsingleFile(Encoding encoding, string bencodeValue);

        MultiFileTorrent ReadMultiFile(Encoding encoding, string bencodeValue);
    }
}