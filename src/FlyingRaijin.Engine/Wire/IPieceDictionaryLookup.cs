namespace FlyingRaijin.Engine.Wire
{
    internal interface IPieceDictionaryLookup
    {
        bool TryGet(int pieceIndex, out PieceBlock block);
    }
}