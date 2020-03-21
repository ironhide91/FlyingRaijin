namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public interface IClrObject
    {

    }

    public interface IClrObject<T> : IClrObject
    {
        T Value { get; }
    }
}