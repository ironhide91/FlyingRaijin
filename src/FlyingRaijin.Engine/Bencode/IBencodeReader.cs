using FlyingRaijin.Engine.Torrent;
using System.IO;
using System.Text;

namespace FlyingRaijin.Engine.Bencode
{
    public interface IBencodeReader
    {
        SingleFileTorrent ReadsingleFile(Encoding encoding, Stream stream);

        MultiFileTorrent ReadMultiFile(Encoding encoding, Stream stream);
    }
}