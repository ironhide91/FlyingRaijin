namespace FlyingRaijin.Bencode.BObject
{
    public sealed class BInteger : BObject<long>
    {
        internal BInteger(IBObject parent, long value) : base(parent, value)
        {

        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}