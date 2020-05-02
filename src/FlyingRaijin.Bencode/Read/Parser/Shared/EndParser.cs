using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void EndParser(ParserContext context, NodeBase ast)
        {
            //context.HasTokens();
            context.Match(EndNode.Instance.Character);

            ast.Children.Add(EndNode.Instance);
        }
    }
}