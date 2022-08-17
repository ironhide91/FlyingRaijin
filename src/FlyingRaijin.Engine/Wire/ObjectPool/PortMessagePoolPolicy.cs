using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PortMessagePoolPolicy : IPooledObjectPolicy<PortMessage>
    {
        internal static readonly PortMessagePoolPolicy Instance = new PortMessagePoolPolicy();

        public PortMessage Create()
        {
            return new PortMessage();
        }

        public bool Return(PortMessage obj)
        {
            obj.Port = default;

            return true;
        }
    }
}