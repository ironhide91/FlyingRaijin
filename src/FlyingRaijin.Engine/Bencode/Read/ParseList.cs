using FlyingRaijin.Bencode.BObject;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ErrorType ParseList(ref IBObject parent, IBObject key)
        {
            var val = new List<IBObject>();

            var bList = new BList(parent, val);

            if (parent is BDictionary)
            {
                if (key is BString @string)
                {
                    (parent as BDictionary).Value.Add(@string, bList);
                    parent = bList;
                    return ErrorType.None;
                }

                return ErrorType.KeyShouldBeString;
            }

            if (parent is BList)
            {
                (parent as BList).Value.Add(bList);
                parent = bList;
                return ErrorType.None;
            }

            return ErrorType.Unknown;
        }
    }
}