namespace FlyingRaijin.Bencode.Read
{
    public enum ErrorType
    {
        None,
        IntegerMustEndWithE,
        IntegerTrailingZeroAfterZero,
        StringInvalid,
        KeyShouldBeString,
        Unknown
    }
}