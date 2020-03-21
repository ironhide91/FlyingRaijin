using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void NegativeSignParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(NegativeSignNode.NegativeSignByte);

            var node = new NegativeSignNode();

            ast.Children.Add(node);
        }
    }
}