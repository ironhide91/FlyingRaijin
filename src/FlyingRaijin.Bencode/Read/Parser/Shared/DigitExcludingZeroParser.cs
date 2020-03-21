using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
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