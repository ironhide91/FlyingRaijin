namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public class BString : IClrObject<string>
    {
        public BString(byte[] underlyingBytes, int length, string value)
        {
            UnderlyingBytes = underlyingBytes;
            Length = length;
            Value = value;
        }

        public readonly byte[] UnderlyingBytes;

        public int Length { get; }

        public string Value { get; }
    }
}