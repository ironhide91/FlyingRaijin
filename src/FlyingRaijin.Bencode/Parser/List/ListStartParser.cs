using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.List;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.List
{
    public sealed class ListStartParser : TerminalParserBase<ListStartNode>
    {
        public static ListStartParser Parser => new ListStartParser();

        private ListStartParser()
        {

        }

        public override Production ProductionType => Production.LIST_START;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(ListStartNode.ListStartTerminalByte);

            var node = new ListStartNode();

            ast.Children.Add(node);
        }
    }
}