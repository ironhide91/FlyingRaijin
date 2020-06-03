using System;
using System.Text;

namespace FlyingRaijin.Test
{
    public static class Extensions
    {
        public static ReadOnlySpan<byte> GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str).AsSpan();
        }
    }
}