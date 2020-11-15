using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FlyingRaijin.Bencode.BObject
{
    public sealed class BString : BObject<byte[]>
    {
        public BString(IBObject parent, byte[] value) : base(parent, value)
        {
            
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BString && ((BString)obj).Value.Equals(Value))
                return true;

            return false;
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Value);
        }
    }
}