using System;

namespace FlyingRaijin.Bencode.Exceptions
{
    public abstract class BencodeException : Exception
    {
        public BencodeException(string message) : base(message)
        {

        }
    }
}