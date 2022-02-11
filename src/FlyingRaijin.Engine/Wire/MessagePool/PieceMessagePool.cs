using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PieceMessagePool : MessagePool<PieceMessage>
    {
        internal static readonly PieceMessagePool Pool = new PieceMessagePool(PieceMessagePoolPolicy.Instance);

        private PieceMessagePool(IPooledObjectPolicy<PieceMessage> policy) : base(policy, 1000)
        {

        }
    }
}