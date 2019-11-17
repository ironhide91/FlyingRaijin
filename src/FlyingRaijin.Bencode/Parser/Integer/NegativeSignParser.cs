using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;

namespace FlyingRaijin.Bencode.Parser
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