using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Shared;
using System;

namespace FlyingRaijin.Bencode.Parser.Integer
{
    public sealed class IntegerParser : NonTerminalParserBase<IntegerNode>
    {
        public static IntegerParser Parser =>
            new IntegerParser(Pack(ZeroParser.Parser, NegativeSignParser.Parser, NumberParser.Parser));

        private IntegerParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.INTEGER;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();

            var node = new IntegerNode();
            ast.Children.Add(node);

            if (context.LookAheadByte == '0')
            {
                Parsers[Production.ZERO].Parse(context, node);
                return;
            }

            if (context.LookAheadByte == '-')
            {
                Parsers[Production.NEGATIVE_SIGN].Parse(context, node);
            }

            if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadByte))
            {
                Parsers[Production.NUMBER].Parse(context, node);
            }
            else
            {
                throw new Exception("");
            }
        }
    }
}