using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void DigitExcludingZeroParser(ParseContext context, NodeBase ast)
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