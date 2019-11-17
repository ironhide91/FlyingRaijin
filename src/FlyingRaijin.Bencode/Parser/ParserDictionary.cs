using FlyingRaijin.Bencode.Parser.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlyingRaijin.Bencode.Parser
{
    public class ParserDictionary : ReadOnlyDictionary<Production, IParser>
    {
        public ParserDictionary(IDictionary<Production, IParser> dictionary) : base(dictionary)
        {

        }
    }
}