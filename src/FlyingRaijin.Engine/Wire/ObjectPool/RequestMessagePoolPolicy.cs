using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class RequestMessagePoolPolicy : IPooledObjectPolicy<RequestMessage>
    {
        internal static readonly RequestMessagePoolPolicy Instance = new RequestMessagePoolPolicy();

        public RequestMessage Create()
        {
            return new RequestMessage();
        }

        public bool Return(RequestMessage obj)
        {
            obj.Index = default;
            obj.Begin = default;
            obj.Length = default;

            return true;
        }
    }
}