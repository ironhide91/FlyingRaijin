using System;

namespace FlyingRaijin.Bencode.Read.Exceptions
{
    public abstract class BencodeException : Exception
    {
        public BencodeException(string message) : base(message)
        {

        }
    }
}