using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Dictionary;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void DictionaryStartParser(ParserContext context, NodeBase ast)
        {
            context.Match(DictionaryStartNode.Instance.Character);

            ast.Children.Add(DictionaryStartNode.Instance);
        }
    }
}