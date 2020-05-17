using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;
using FlyingRaijin.Bencode.Read.Converter;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void BencodeStringParser(ParserContext context, NodeBase ast)
        {
            var node = new BencodeStringNode();
            ast.Children.Add(node);

            NumberParser(context, node);
            StringLengthPrefixParser(context, node);

            var chars = NumberConverter.Convert(node.Children[0]);
            var length = int.Parse(new string(chars));

            if (length > 0)
                StringParser(context, node, ref length);
        }
    }
}