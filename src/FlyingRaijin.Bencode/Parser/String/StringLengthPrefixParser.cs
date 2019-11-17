using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.String
{
    public sealed class StringLengthPrefixParser : TerminalParserBase<ZeroNode>
    {
        public static StringLengthPrefixParser Parser => new StringLengthPrefixParser();

        private StringLengthPrefixParser()
        {

        }

        public override Production ProductionType => Production.STRING_LENGTH_PREFIX;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(StringLengthPrefixNode.LENGTH_PREFIX);

            var node = new StringLengthPrefixNode();
            ast.Children.Add(node);
        }
    }
}