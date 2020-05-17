using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void NumberParser(ParserContext context, NodeBase ast)
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

            DigitExcludingZeroParser(context, node);

            if (context.IsMatch(DigitExcludingZeroNode.DigitsExcludingZero))
            {
                DigitExcludingZeroParser(context, node);
                NumberParser(context, node);
            }

            if (context.IsMatch(ZeroNode.Instance.Character))
            {
                ZeroParser(context, node);
                NumberParser(context, node);
            }
        }
    }
}