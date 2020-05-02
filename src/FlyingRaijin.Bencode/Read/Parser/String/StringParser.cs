using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public interface IStringParser
    {
        int BytesToProcess { get; set; }
    }

    public static partial class DelegateParsers
    {
        public static void StringParser(ParserContext context, NodeBase ast, ref int bytesToProcess)
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
                CharParser(context, node);
            }


            //byte[] bytes = new byte[bytesToProcess];

            //context.Advance(bytesToProcess, bytes);
        }
    }
}