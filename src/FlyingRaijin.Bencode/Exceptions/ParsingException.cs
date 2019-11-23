namespace FlyingRaijin.Bencode.Exceptions
{
    public sealed class ParsingException : BencodeException
    {
        private ParsingException(string message) : base(message)
        {

        }

        public static ParsingException Create(long exceptionAt)
        {
            return new ParsingException($"Exception while parsing at position {exceptionAt} within the stream.");
        }
    }
}