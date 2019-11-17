using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void IntegerStartParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(IntegerStartNode.IntegerStartNonTerminalByte);

            var node = new IntegerStartNode();

            ast.Children.Add(node);
        }
    }
}