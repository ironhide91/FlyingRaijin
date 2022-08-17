using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PieceBlockPool : DefaultObjectPool<PieceBlock>
    {
        internal static readonly PieceBlockPool Pool = new PieceBlockPool(PieceBlockPoolPolicy.Instance);

        private PieceBlockPool(IPooledObjectPolicy<PieceBlock> policy) : base(policy, 1000)
        {

        }
    }
}