namespace FlyingRaijin.Bencode.Read.Exceptions
{
    public sealed class ConverterException : BencodeException
    {
        private ConverterException(string message) : base(message)
        {

        }

        public static ConverterException Create(string message)
        {
            return new ConverterException(message);
        }
    }
}