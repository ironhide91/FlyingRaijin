namespace FlyingRaijin.Bencode.BObject
{
    public interface IBObject
    {
        IBObject Parent { get; }
    }

    public interface IBObject<T> : IBObject
    {
        T Value { get; }
    }
}