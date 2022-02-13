using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class HaveMessagePool : MessagePool<HaveMessage>
    {
        internal static readonly HaveMessagePool Pool = new HaveMessagePool(HaveMessagePoolPolicy.Instance);

        private HaveMessagePool(IPooledObjectPolicy<HaveMessage> policy) : base(policy, 1000)
        {

        }
    }
}