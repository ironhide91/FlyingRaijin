using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;
using System;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void CharParser(ParserContext context, NodeBase ast)
        {
            var character = context.LookAheadChar;

            context.Match(character);

            var node = new CharNode(character);

            ast.Children.Add(node);
        }
    }
}