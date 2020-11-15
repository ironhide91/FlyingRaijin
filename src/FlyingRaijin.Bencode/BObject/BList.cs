using System.Collections.Generic;

namespace FlyingRaijin.Bencode.BObject
{
    public sealed class BList : BObject<IList<IBObject>>
    {
        public BList(IBObject parent, IList<IBObject> value) : base(parent, value)
        {

        }
    }   
}