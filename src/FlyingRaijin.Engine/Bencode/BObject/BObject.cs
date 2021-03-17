namespace FlyingRaijin.Bencode.BObject
{
    public class BObject<T> : IBObject<T>
    {
        protected BObject(IBObject p, T v)
        {
            parent = p;
             value = v;
        }

        private readonly IBObject parent;

        private readonly T value;

        public IBObject Parent => parent;

        public T Value => value;
    }
}