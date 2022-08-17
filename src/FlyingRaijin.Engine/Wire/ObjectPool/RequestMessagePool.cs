using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class RequestMessagePool : MessagePool<RequestMessage>
    {
        internal static readonly RequestMessagePool Pool = new RequestMessagePool(RequestMessagePoolPolicy.Instance);

        private RequestMessagePool(IPooledObjectPolicy<RequestMessage> policy) : base(policy, 1000)
        {

        }
    }
}