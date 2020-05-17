using FlyingRaijin.Engine.Torrent;
using System.IO;

namespace FlyingRaijin.Engine.Bencode
{
    public interface IBencodeReader
    {
        SingleFileTorrent ReadsingleFile(Stream stream);

        MultiFileTorrent ReadMultiFile(Stream stream);
    }
}