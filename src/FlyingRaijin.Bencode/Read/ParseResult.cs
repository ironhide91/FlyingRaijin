using FlyingRaijin.Bencode.BObject;

namespace FlyingRaijin.Bencode.Read
{
    public class ParseResult
    {
        public ParseResult(ErrorType error, IBObject bObject)
        {
            Error = error;
            BObject = bObject;
        }

        public readonly ErrorType Error;

        public readonly IBObject BObject;
    }
}