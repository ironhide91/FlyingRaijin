using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PieceMessagePoolPolicy : IPooledObjectPolicy<PieceMessage>
    {
        internal static readonly PieceMessagePoolPolicy Instance = new PieceMessagePoolPolicy();

        public PieceMessage Create()
        {
            return new PieceMessage();
        }

        public bool Return(PieceMessage obj)
        {
            obj.Index = default;
            obj.Begin = default;
            obj.Data = default;

            return true;
        }
    }
}