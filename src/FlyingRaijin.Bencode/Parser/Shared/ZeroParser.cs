using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.Shared
{
    public sealed class ZeroParser : TerminalParserBase<ZeroNode>
    {
        public static ZeroParser Parser => new ZeroParser();

        private ZeroParser()
        {

        }

        public override Production ProductionType => Production.ZERO;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(ZeroNode.ZeroDigitByte);

            var node = new ZeroNode();

            ast.Children.Add(node);
        }
    }
}