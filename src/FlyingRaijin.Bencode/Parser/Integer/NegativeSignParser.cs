using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.Integer
{
    public sealed class NegativeSignParser : TerminalParserBase<NegativeSignNode>
    {
        public static NegativeSignParser Parser => new NegativeSignParser();

        private NegativeSignParser()
        {

        }
        
        public override Production ProductionType => Production.NEGATIVE_SIGN;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(NegativeSignNode.NegativeSignByte);

            var node = new NegativeSignNode();

            ast.Children.Add(node);
        }
    }
}