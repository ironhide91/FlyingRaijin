﻿using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FlyingRaijin.Bencode.BObject
{
    public class BString : BObject<byte[]>
    {
        public BString(IBObject parent, byte[] value) : base(parent, value)
        {
            
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Value);
        }
    }
}