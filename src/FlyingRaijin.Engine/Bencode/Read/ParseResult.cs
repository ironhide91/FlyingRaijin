using FlyingRaijin.Bencode.BObject;

namespace FlyingRaijin.Bencode.Read
{
    public class ParseResult
    {
        public ParseResult(ErrorType error, IBObject bObject, int infoBeginIndex = -1, int infoEndIndex = -1)
        {
                     Error = error;
                   BObject = bObject;
            InfoBeginIndex = infoBeginIndex;
              InfoEndIndex = infoEndIndex;
        }

        public readonly ErrorType Error;

        public readonly IBObject BObject;

        public readonly int InfoBeginIndex;

        public readonly int InfoEndIndex;
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