namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public readonly struct BString : IClrObject<string>
    {
        public BString(int length, string value)
        {
            Length = length;
            Value = value;
        }

        public int Length { get; }

        public string Value { get; }
    }
}