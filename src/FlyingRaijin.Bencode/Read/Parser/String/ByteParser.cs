using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;

namespace FlyingRaijin.Bencode.Read.Parser
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