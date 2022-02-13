using FlyingRaijin.Engine.Torrent;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class CompletePiece
    {
        internal CompletePiece(MetaData torrent, PieceBlock block)
        {
            Torrent = torrent;
            Block = block;
        }

        internal readonly MetaData Torrent;
        internal readonly PieceBlock Block;
    }
}