﻿using System.Collections.Immutable;

namespace FlyingRaijin.Bencode.Read.ClrObject
{
    public readonly struct BDictionary : IClrObject<ImmutableDictionary<string, IClrObject>>
    {
        public BDictionary(ImmutableDictionary<string, IClrObject> value)
        {
            Value = value;
        }

        public ImmutableDictionary<string, IClrObject> Value { get; }
    }
}