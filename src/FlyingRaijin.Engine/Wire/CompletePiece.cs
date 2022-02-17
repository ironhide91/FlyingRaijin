using FlyingRaijin.Engine.Torrent;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class CompletePiece
    {
        internal CompletePiece(MetaData metaData, int pieceIndex, PieceBlock block)
        {
            MetaData = metaData;
            PieceIndex = pieceIndex;
            Block = block;
        }

        internal readonly MetaData MetaData;
        internal readonly int PieceIndex;
        internal readonly PieceBlock Block;
    }
}