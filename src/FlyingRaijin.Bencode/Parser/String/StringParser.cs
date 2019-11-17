using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.String
{
    public interface IStringParser
    {
        int BytesToProcess { get; set; }
    }

    public sealed class StringParser : NonTerminalParserBase<NumberNode>, IStringParser
    {
        public static StringParser Parser =>
            new StringParser(Pack(ByteParser.Parser));

        private StringParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.STRING;

        public int BytesToProcess { get; set; } = 0;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            StringNode node;

            if (ast is StringNode)
            {
                node = (StringNode)ast;
            }
            else
            {
                node = new StringNode();
                ast.Children.Add(node);
            }

            while (BytesToProcess-- > 0)
            {
                Parsers[Production.BYTE].Parse(context, node);
            }
        }
    }
}