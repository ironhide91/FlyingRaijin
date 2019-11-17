namespace FlyingRaijin.Bencode.ClrObject
{
    public interface IClrObject
    {

    }

    public interface IClrObject<T> : IClrObject
    {
        T Value { get; }
    }
}