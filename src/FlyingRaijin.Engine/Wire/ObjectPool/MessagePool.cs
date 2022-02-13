using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal abstract class MessagePool<T> : DefaultObjectPool<T> where T : class, IMessage
    {
        protected MessagePool(IPooledObjectPolicy<T> policy, int maximumRetained) : base(policy, maximumRetained)
        {

        }
    }
}