using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.List;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void ListStartParser(ParserContext context, NodeBase ast)
        {
            //context.HasTokens();
            context.Match(ListStartNode.Instance.Character);

            ast.Children.Add(ListStartNode.Instance);
        }
    }
}