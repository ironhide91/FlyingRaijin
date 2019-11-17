using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.List;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void ListStartParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(ListStartNode.ListStartTerminalByte);

            var node = new ListStartNode();

            ast.Children.Add(node);
        }
    }
}