using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FlyingRaijin.Bencode.BObject
{
    public class BDictionary : BObject<IDictionary<BString, IBObject>>
    {
        public BDictionary(IBObject parent, IDictionary<BString, IBObject> value) : base(parent, value)
        {

        }

        public bool ContainsKey(string key)
        {
            if (dict == null || dict.Count == 0)
                return false;

            return dict.ContainsKey(key);
        }

        public IBObject this[string key]
        {
            get
            {
                if (dict.ContainsKey(key))
                    return dict[key];

                return null;
            }
        }

        public void SyncInternalStringDictionary()
        {
            dict.Clear();

            foreach (var item in Value)
                dict.Add(item.Key.ToString(), item.Value);
        }       

        private Dictionary<string, IBObject> dict = new Dictionary<string, IBObject>();
    }
}