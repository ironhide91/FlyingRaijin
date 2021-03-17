using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FlyingRaijin.Bencode.BObject
{
    public sealed class BString : BObject<ReadOnlyMemory<byte>>
    {
        public readonly string StringValue;

        public BString(IBObject parent, ReadOnlyMemory<byte> value) : base(parent, value)
        {
            StringValue = Encoding.UTF8.GetString(value.Span);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BString @string && @string.Value.Equals(Value))
                return true;

            return false;
        }

        public override string ToString()
        {
            return StringValue;
        }        
    }
}