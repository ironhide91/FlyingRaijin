using FlyingRaijin.Bencode.Read.ClrObject;
using System;
using System.Globalization;

namespace FlyingRaijin.Client.Torrent
{
    public static class TypeExtensions
    {
        public static T GetValue<T>(this IClrObject obj)
        {
            return ((IClrObject<T>)obj).Value;
        }

        public static T Cast<T>(this IClrObject obj)
        {
            return (T)obj;
        }

        public static T GetValue<T>(this BDictionary dictionary, string key)
        {            
            return dictionary.Value[key].Cast<T>();
        }

        public static T TryGetValue<T>(this BDictionary dictionary, string key)
        {
            T obj = default;

            IClrObject clrObj;
            if (dictionary.Value.TryGetValue(key, out clrObj))
            {
                obj = clrObj.Cast<T>();
            }

            return obj;
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        public static ReadOnlySpan<byte> HexToByteArray(this string hexString)
        {            
            var chars = hexString.AsSpan();

            if ((chars[0] != '0') && (chars[1] != 'x'))
            {
                throw new Exception("");
            }

            int index = 2; 
            int count = chars.Length / 2;
            var bytes = new byte[count];

            int byteArrayIndex = 0;

            int end = count - 2;

            while (index <= end)
            {
                bytes[byteArrayIndex++] = byte.Parse(chars.Slice(index, 2), NumberStyles.HexNumber);
                index += 2;
            }

            return bytes;
        }
    }
}