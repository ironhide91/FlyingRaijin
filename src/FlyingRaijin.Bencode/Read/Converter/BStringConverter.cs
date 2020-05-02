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

        public BString Convert(BencodeStringNode node)
        {
            BString result;

            try
            {
                var intChars = new string(NumberConverter.Convert(node.Children[0]));

                var length = int.Parse(intChars);

                var isConsitent = (length == node.Children[2].Children.Count);

                var chars = node.Children[2].Children.Cast<CharNode>().Select(x => x.Character).ToArray();

                result = new BString(length, new string(chars));
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