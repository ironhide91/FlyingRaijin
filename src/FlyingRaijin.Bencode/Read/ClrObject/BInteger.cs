namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public readonly struct BInteger : IClrObject<long>
    {
        public BInteger(long value)
        {
            Value = value;
        }

        public long Value { get; }
    }
}