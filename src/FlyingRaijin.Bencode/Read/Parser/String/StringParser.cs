using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;
using System.Linq;

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

            if (bytesToProcess == 32400)
            {
                byte[] bytes = new byte[bytesToProcess];

                context.Advance(bytes.Length, bytes);

                //var temp1 = System.Convert.ToBase64String(bytes.Take(20).ToArray());

                //var temp = System.Text.Encoding.UTF8.GetString(bytes.Take(20).ToArray());
            }
            else
            {
                while (bytesToProcess-- > 0)
                {
                    CharParser(context, node);
                }
            }
        }
    }
}