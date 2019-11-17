using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Dictionary;
using FlyingRaijin.Bencode.Parser.Base;

namespace FlyingRaijin.Bencode.Parser.Dictionary
{
    public sealed class DictionaryStartParser : TerminalParserBase<DictionaryStartNode>
    {
        public static DictionaryStartParser Parser => new DictionaryStartParser();

        private DictionaryStartParser()
        {

        }

        public override Production ProductionType => Production.DICTIONARY_START;

        public override void Parse(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(DictionaryStartNode.DictionaryStartTerminalByte);

            var node = new DictionaryStartNode();

            ast.Children.Add(node);
        }
    }
}