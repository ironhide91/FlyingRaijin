﻿using System.Collections.Generic;

namespace FlyingRaijin.Bencode.ClrObject
{
    public sealed class BList : IClrObject<IReadOnlyList<IClrObject>>
    {
        public BList(IReadOnlyList<IClrObject> value)
        {
            Value = value;
        }

        public IReadOnlyList<IClrObject> Value { get; private set; }
    }
}