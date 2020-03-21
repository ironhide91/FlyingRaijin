using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;

namespace FlyingRaijin.Bencode.Read.Parser
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