using System.Collections.Generic;

namespace FlyingRaijin.Bencode.ClrObject
{
    public sealed class BDictionary : IClrObject<IReadOnlyDictionary<string, IClrObject>>
    {
        public BDictionary(IReadOnlyDictionary<string, IClrObject> value)
        {
            Value = value;
        }

        public IReadOnlyDictionary<string, IClrObject> Value { get; private set; }
    }
}