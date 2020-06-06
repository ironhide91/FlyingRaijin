namespace FlyingRaijin.Bencode.Read
{
    public enum ErrorType
    {
        None,
        Unknown,
        // Integer
        IntegerInvalid,
        IntegerOutOfInt64Range,
        // String
        StringLessCharsThanSpecified,
        StringInvalidStringLength,
        StringInvalid,
        KeyShouldBeString        
    }
}