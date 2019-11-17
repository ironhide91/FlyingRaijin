using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Dictionary;

namespace FlyingRaijin.Bencode.Parser
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