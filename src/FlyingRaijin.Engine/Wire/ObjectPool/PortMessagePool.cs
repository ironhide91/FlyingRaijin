using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PortMessagePool : MessagePool<PortMessage>
    {
        internal static readonly PortMessagePool Pool = new PortMessagePool(PortMessagePoolPolicy.Instance);

        private PortMessagePool(IPooledObjectPolicy<PortMessage> policy) : base(policy, 100)
        {

        }
    }
}