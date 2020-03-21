using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void ZeroParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(ZeroNode.ZeroDigitByte);

            var node = new ZeroNode();

            ast.Children.Add(node);
        }
    }
}