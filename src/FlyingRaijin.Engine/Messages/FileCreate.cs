using FlyingRaijin.Engine.Torrent;

namespace FlyingRaijin.Engine.Messages
{
    internal class FileCreate
    {
        internal FileCreate(InfoHash infoHash, int pieceIndex, string path)
        {
            InfoHash = infoHash;
            PieceIndex = pieceIndex;
            Path = path;
        }

        internal readonly InfoHash InfoHash;
        internal readonly int PieceIndex;
        internal readonly string Path;
    }
}