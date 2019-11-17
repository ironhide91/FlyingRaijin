﻿namespace FlyingRaijin.Bencode.ClrObject
{
    public sealed class BInteger : IClrObject<long>
    {
        public BInteger(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }
    }
}