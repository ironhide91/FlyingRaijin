namespace FlyingRaijin.Bencode.Read
{
    public enum ErrorType
    {
        None,
        // Integer
        IntegerMinimumLemgthMustBe3,
        IntegerMustEndWithE,
        IntegerLeadingZero,
        IntegerNegativeOnly,
        IntegerNegativeZero,
        IntegerMultipleNegative,
        IntegerOutOfInt64Range,
        // String
        StringInvalid,
        KeyShouldBeString,
        Unknown
    }
}