using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class HaveMessagePoolPolicy : IPooledObjectPolicy<HaveMessage>
    {
        internal static readonly HaveMessagePoolPolicy Instance = new HaveMessagePoolPolicy();

        public HaveMessage Create()
        {
            return new HaveMessage();
        }

        public bool Return(HaveMessage obj)
        {
            obj.Index = default;

            return true;
        }
    }
}