using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void NegativeSignParser(ParserContext context, NodeBase ast)
        {
            context.Match(NegativeSignNode.Instance.Character);

            ast.Children.Add(NegativeSignNode.Instance);
        }
    }
}