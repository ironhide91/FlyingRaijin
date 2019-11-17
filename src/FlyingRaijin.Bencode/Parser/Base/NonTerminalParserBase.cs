using FlyingRaijin.Bencode.Ast.Base;
using System.Linq;

namespace FlyingRaijin.Bencode.Parser.Base
{
    public abstract class NonTerminalParserBase<T> : IParser where T : NonTerminalNodeBase
    {
        protected NonTerminalParserBase(ParserDictionary dependentParsers)
        {

        }

        protected abstract ParserDictionary Parsers { get; set; }
        
        public abstract Production ProductionType { get; }

        public abstract void Parse(ParseContext context, NodeBase ast);

        public static ParserDictionary Pack(params IParser[] processors)
        {
            var dict = processors.ToDictionary(kv => kv.ProductionType, kv => kv);

            return new ParserDictionary(dict);
        }
    }
}