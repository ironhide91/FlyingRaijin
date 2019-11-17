using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.String;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void ByteParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();

            var nextByte = context.LookAheadByte;

            context.Match(nextByte);

            var node = new ByteNode(nextByte);

            ast.Children.Add(node);
        }
    }
}