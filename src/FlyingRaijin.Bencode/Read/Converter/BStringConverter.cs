using FlyingRaijin.Bencode.Read.Ast.String;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read.Converter;
using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class BStringConverter : IClrObjectConverter<BencodeStringNode, BString>
    {
        public static BStringConverter Converter => new BStringConverter();

        private BStringConverter()
        {

        }

        public BString Convert(Encoding encoding, BencodeStringNode node)
        {
            BString result;

            try
            {
                var numberBytes = NumberConverter.Convert(node.Children[0]).ToArray();

                var length = int.Parse(encoding.GetString(numberBytes));

                var isConsitent = (length == node.Children[2].Children.Count);

                var bytes = node.Children[2].Children.Cast<ByteNode>().Select(x => x.Byte).ToArray();

                result = new BString(length, encoding.GetString(bytes));
            }
            catch (Exception e)
            {
                throw ConverterException.Create(
                        $"{nameof(BStringConverter)} - An error occurred while converting Bencode Abstract Sytax Tree.");
            }

            return result;
        }
    }
}