using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.Shared
{
    public sealed class NumberParser : NonTerminalParserBase<NumberNode>
    {
        public static NumberParser Parser =>
            new NumberParser(Pack(DigitExcludingZeroParser.Parser, ZeroParser.Parser));

        private NumberParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.NUMBER;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            NumberNode node = null;

            if (ast is NumberNode)
            {
                node = (NumberNode)ast;
            }
            else
            {
                node = new NumberNode();
                ast.Children.Add(node);
            }

            context.HasTokens();
            Parsers[Production.DIGIT_EXCULUDING_ZERO].Parse(context, node);

            context.HasTokens();
            if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadByte))
            {
                Parsers[Production.DIGIT_EXCULUDING_ZERO].Parse(context, node);
                Parse(context, node);
            }

            context.HasTokens();
            if (context.LookAheadByte == ZeroNode.ZeroDigitByte)
            {
                Parsers[Production.ZERO].Parse(context, node);
                Parse(context, node);
            }
        }
    }
}