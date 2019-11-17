using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;

namespace FlyingRaijin.Bencode.Parser
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