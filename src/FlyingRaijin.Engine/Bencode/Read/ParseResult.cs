using FlyingRaijin.Bencode.BObject;

namespace FlyingRaijin.Bencode.Read
{
    public class ParseResult
    {
        public ParseResult(ErrorType error, IBObject bObject, int infoBeginIndex, int infoEndIndex)
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

    public class ParseResult<T> : ParseResult where T : IBObject
    {
        public ParseResult(ErrorType error, T bObject, int infoBeginIndex, int infoEndIndex)
            : base(error, bObject, infoBeginIndex, infoEndIndex)
        {
            Error = error;
            BObject = bObject;
        }

        public readonly new ErrorType Error;

        public readonly new T BObject;
    }
}