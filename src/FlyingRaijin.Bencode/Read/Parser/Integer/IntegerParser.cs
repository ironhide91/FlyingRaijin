using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using FlyingRaijin.Bencode.Read.Exceptions;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void IntegerParser(ParserContext context, NodeBase ast)
        {
            var node = new IntegerNode();
            ast.Children.Add(node);

            if (context.IsMatch(ZeroNode.Instance.Character))
            {
                ZeroParser(context, node);
                return;
            }

            if (context.IsMatch(NegativeSignNode.Instance.Character))
            {
                NegativeSignParser(context, node);
            }

            if (context.IsMatch(DigitExcludingZeroNode.DigitsExcludingZero))
            {
                NumberParser(context, node);
            }
            else
            {
                throw ParsingException.Create("Invalid Bencode.");
            }
        }
    }
}