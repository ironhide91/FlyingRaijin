using FlyingRaijin.Bencode.BObject;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class Parser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ErrorType ParseDictionary(ref IBObject parent, IBObject key)
        {
            var val = new Dictionary<BString, IBObject>();

            var bdict = new BDictionary(parent, val);

            if (parent is BDictionary)
            {
                if (key is BString)
                {
                    (parent as BDictionary).Value.Add((BString)key, bdict);
                    parent = bdict;
                    return ErrorType.None;
                }

                return ErrorType.KeyShouldBeString;
            }

            if (parent is BList)
            {
                (parent as BList).Value.Add(bdict);
                parent = bdict;
                return ErrorType.None;
            }

            return ErrorType.Unknown;
        }
    }
}