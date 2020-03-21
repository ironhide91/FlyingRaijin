using System.Collections.Generic;
using System.Collections.Immutable;

namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public readonly struct BList : IClrObject<ImmutableList<IClrObject>>
    {
        public BList(ImmutableList<IClrObject> value)
        {
            Value = value;
        }

        public ImmutableList<IClrObject> Value { get; }
    }
}