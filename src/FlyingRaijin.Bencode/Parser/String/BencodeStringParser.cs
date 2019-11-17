using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.Converter;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void BencodeStringParser(ParseContext context, NodeBase ast)
        {
            var node = new BencodeStringNode();
            ast.Children.Add(node);

            NumberParser(context, node);
            StringLengthPrefixParser(context, node);

            var lengthBytes = NumberConverter.Convert(node.Children.ElementAt(0));
            var length = int.Parse(Encoding.UTF8.GetString(lengthBytes));

            StringParser(context, node, ref length);
        }
    }
}