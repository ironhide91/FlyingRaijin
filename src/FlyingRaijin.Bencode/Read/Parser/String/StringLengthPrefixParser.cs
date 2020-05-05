using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void StringLengthPrefixParser(ParserContext context, NodeBase ast)
        {
            context.Match(StringLengthPrefixNode.Instance.Character);

            ast.Children.Add(StringLengthPrefixNode.Instance);
        }
    }
}