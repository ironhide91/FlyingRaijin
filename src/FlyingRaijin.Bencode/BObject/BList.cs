using System.Collections.Generic;

namespace FlyingRaijin.Bencode.BObject
{
    public class BList : BObject<IList<IBObject>>
    {
        public BList(IBObject parent, IList<IBObject> value) : base(parent, value)
        {

        }
    }   
}