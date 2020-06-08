using FlyingRaijin.Engine.Torrent;
using System;

namespace FlyingRaijin.Engine.Bencode
{
    public interface IBencodeReader
    {
        SingleFileTorrent ReadsingleFile(ReadOnlySpan<byte> bytes);

        SingleFileTorrent ReadsingleFile(string filePath);

        MultiFileTorrent ReadMultiFile(ReadOnlySpan<byte> bytes);

        MultiFileTorrent ReadMultiFile(string filePath);
    }
}