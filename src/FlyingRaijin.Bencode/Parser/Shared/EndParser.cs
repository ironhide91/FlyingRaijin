using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.Shared
{
    public sealed class EndParser : TerminalParserBase<EndNode>
    {
        public static EndParser Parser => new EndParser();

        private EndParser()
        {

        }

        public override Production ProductionType => Production.END;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(EndNode.IntegerEndNonTerminalByte);

            var node = new EndNode();

            ast.Children.Add(node);
        }
    }
}