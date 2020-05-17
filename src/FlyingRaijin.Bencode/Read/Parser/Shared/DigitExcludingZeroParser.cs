using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void DigitExcludingZeroParser(ParserContext context, NodeBase ast)
        {
            if (context.IsMatch(DigitExcludingZeroNode.DigitsExcludingZero))
            {
                var nonZeroDigit = context.LookAheadChar;

                context.Match(nonZeroDigit);

                var node = new DigitExcludingZeroNode(nonZeroDigit);

                ast.Children.Add(node);
            }
        }
    }
}