using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.String;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void StringLengthPrefixParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(StringLengthPrefixNode.LENGTH_PREFIX);

            var node = new StringLengthPrefixNode();
            ast.Children.Add(node);
        }
    }
}