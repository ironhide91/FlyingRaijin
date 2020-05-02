using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;
using System;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void CharParser(ParserContext context, NodeBase ast)
        {
            //var nextChar = context.LookAheadChar;            

            //context.Match(nextChar);

            var b = context.Reader.Read();

            //var node = new CharNode(BitConverter.ToCharParse(b));
            var node = new CharNode((char)b);

            ast.Children.Add(node);
        }
    }
}