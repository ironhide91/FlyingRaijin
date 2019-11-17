using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser
{
    public sealed class DigitExcludingZeroParser : TerminalParserBase<DigitExcludingZeroNode>
    {
        public static DigitExcludingZeroParser Parser => new DigitExcludingZeroParser();

        private DigitExcludingZeroParser()
        {

        }
        
        public override Production ProductionType => Production.DIGIT_EXCULUDING_ZERO;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();            

            var chr = context.LookAheadByte;

            if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadByte))
            {
                context.Match(context.LookAheadByte);

                var node = new DigitExcludingZeroNode(chr);

                ast.Children.Add(node);
            }
        }
    }
}