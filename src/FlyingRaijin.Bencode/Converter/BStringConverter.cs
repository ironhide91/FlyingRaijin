using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Converter;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Parser
{
    public sealed class BStringConverter : IClrObjectConverter<BencodeStringNode, BString>
    {
        public static BStringConverter Converter => new BStringConverter();

        private BStringConverter()
        {

        }

        public BString Convert(Encoding encoding, BencodeStringNode node)
        {
            var numberBytes = NumberConverter.Convert(node.Children.ElementAt(0)).ToArray();

            var length = int.Parse(encoding.GetString(numberBytes));

            var isConsitent = (length == node.Children.ElementAt(2).Children.Count);

            var bytes = node.Children.ElementAt(2).Children.Cast<ByteNode>().Select(x => x.Byte).ToArray();

            return new BString(length, encoding.GetString(bytes));
        }
    }
}