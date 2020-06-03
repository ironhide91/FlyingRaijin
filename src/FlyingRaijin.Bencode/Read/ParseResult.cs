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

    public class ParseResult<T> where T : IBObject
    {
        public ParseResult(ErrorType error, T bObject)
        {
            Error = error;
            BObject = bObject;
        }

        public readonly ErrorType Error;

        public readonly T BObject;
    }
}