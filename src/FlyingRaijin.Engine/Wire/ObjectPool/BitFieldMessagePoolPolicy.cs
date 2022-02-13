using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class BitFieldMessagePoolPolicy : IPooledObjectPolicy<BitFieldMessage>
    {
        internal static readonly BitFieldMessagePoolPolicy Instance = new BitFieldMessagePoolPolicy();

        public BitFieldMessage Create()
        {
            return new BitFieldMessage();
        }

        public bool Return(BitFieldMessage obj)
        {
            obj.Index = default;
            obj.Begin = default;
            obj.ReleaseBuffer();

            return true;
        }
    }
}