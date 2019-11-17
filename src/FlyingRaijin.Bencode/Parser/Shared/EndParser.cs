using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void EndParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(EndNode.IntegerEndNonTerminalByte);

            var node = new EndNode();

            ast.Children.Add(node);
        }
    }
}