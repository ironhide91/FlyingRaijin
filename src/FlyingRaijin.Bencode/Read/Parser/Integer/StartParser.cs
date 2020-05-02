using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void IntegerStartParser(ParserContext context, NodeBase ast)
        {
            //context.HasTokens();
            context.Match(IntegerStartNode.Instance.Character);

            ast.Children.Add(IntegerStartNode.Instance);
        }
    }
}