using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Dictionary;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void DictionaryStartParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();
            context.Match(DictionaryStartNode.DictionaryStartTerminalByte);

            var node = new DictionaryStartNode();

            ast.Children.Add(node);
        }
    }
}