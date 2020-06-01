using System.Collections.Generic;

namespace FlyingRaijin.Bencode.BObject
{
    public class BDictionary : BObject<IDictionary<BString, IBObject>>
    {
        public BDictionary(IBObject parent, IDictionary<BString, IBObject> value) : base(parent, value)
        {

        }
    }
}