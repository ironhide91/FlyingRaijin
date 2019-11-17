using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.String
{
    public sealed class ByteParser : IParser
    {
        public static ByteParser Parser => new ByteParser();

        private ByteParser()
        {

        }

        public Production ProductionType => Production.BYTE;

        public void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();

            var nextByte = context.LookAheadByte;

            context.Match(nextByte);

            var node = new ByteNode(nextByte);

            ast.Children.Add(node);
        }
    }
}