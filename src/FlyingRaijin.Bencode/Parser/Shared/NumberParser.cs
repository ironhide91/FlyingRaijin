using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void NumberParser(ParseContext context, NodeBase ast)
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
            DigitExcludingZeroParser(context, node);

            context.HasTokens();
            if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadByte))
            {
                DigitExcludingZeroParser(context, node);
                NumberParser(context, node);
            }

            context.HasTokens();
            if (context.LookAheadByte == ZeroNode.ZeroDigitByte)
            {
                ZeroParser(context, node);
                NumberParser(context, node);
            }
        }
    }
}