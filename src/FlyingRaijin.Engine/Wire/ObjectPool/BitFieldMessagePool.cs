using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class BitFieldMessagePool : MessagePool<BitFieldMessage>
    {
        internal static readonly BitFieldMessagePool Pool = new BitFieldMessagePool(BitFieldMessagePoolPolicy.Instance);

        private BitFieldMessagePool(IPooledObjectPolicy<BitFieldMessage> policy) : base(policy, 1000)
        {

        }
    }
}