namespace FlyingRaijin.Engine.Wire
{
    internal interface IRequestPieceBlock
    {
        void Request(int index, out PieceBlock block);
    }
}