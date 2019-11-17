using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.Integer
{
    public sealed class IntegerStartParser : TerminalParserBase<IntegerStartNode>
    {
        public static IntegerStartParser Parser => new IntegerStartParser();

        private IntegerStartParser()
        {

        }

        public override Production ProductionType => Production.INTEGER_START;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(IntegerStartNode.IntegerStartNonTerminalByte);

            var node = new IntegerStartNode();

            ast.Children.Add(node);
        }
    }
}