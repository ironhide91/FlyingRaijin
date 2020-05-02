using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void DigitExcludingZeroParser(ParserContext context, NodeBase ast)
        {
            //context.HasTokens();

            var chr = context.LookAheadChar;

            if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadChar))
            {
                context.Match(context.LookAheadChar);

                var node = new DigitExcludingZeroNode(chr);

                ast.Children.Add(node);
            }
        }
    }
}