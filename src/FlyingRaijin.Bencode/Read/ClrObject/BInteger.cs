namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public class BInteger : IClrObject<long>
    {
        public BInteger(long value)
        {
            Value = value;
        }

        public long Value { get; }
    }
}