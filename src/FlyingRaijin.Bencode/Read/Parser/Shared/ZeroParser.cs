using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void ZeroParser(ParserContext context, NodeBase ast)
        {
            context.Match(ZeroNode.Instance.Character);           

            ast.Children.Add(ZeroNode.Instance);
        }
    }
}