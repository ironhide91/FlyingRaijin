using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Shared;
using FlyingRaijin.Bencode.Parser.Shared;
using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Parser.Integer
{
    public sealed class BencodeIntegerParser : NonTerminalParserBase<IntegerNode>
    {
        public static BencodeIntegerParser Parser =>
            new BencodeIntegerParser(Pack(IntegerStartParser.Parser, IntegerParser.Parser, EndParser.Parser));

        private BencodeIntegerParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.BENCODED_INTEGER;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            var node = new BencodeIntegerNode();
            ast.Children.Add(node);

            Parsers[Production.INTEGER_START].Parse(context, node);
            Parsers[Production.INTEGER].Parse(context, node);
            Parsers[Production.END].Parse(context, node);
        }
    }
}