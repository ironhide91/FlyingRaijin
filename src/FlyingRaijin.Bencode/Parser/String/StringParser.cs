using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.String;

namespace FlyingRaijin.Bencode.Parser
{
    public interface IStringParser
    {
        int BytesToProcess { get; set; }
    }

    public static partial class DelegateParsers
    {
        public static void StringParser(ParseContext context, NodeBase ast, ref int bytesToProcess)
        {
            StringNode node;

            if (ast is StringNode)
            {
                node = (StringNode)ast;
            }
            else
            {
                node = new StringNode();
                ast.Children.Add(node);
            }

            while (bytesToProcess-- > 0)
            {
                ByteParser(context, node);
            }
        }
    }
}