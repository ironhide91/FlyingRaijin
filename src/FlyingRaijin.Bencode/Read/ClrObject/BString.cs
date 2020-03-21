namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public sealed class BString : IClrObject<string>
    {
        public BString(int length, string value)
        {
            Length = length;
            Value = value;
        }

        public int Length { get; private set; }

        public string Value { get; private set; }
    }
}