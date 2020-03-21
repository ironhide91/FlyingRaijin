using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
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