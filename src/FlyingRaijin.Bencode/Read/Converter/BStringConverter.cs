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
            var intChars = new string(NumberConverter.Convert(node.Children[0]));
            var length = int.Parse(intChars);
            if (length == 0)
                return new BString(null, length, string.Empty);

            var strNode = (StringNode)node.Children[2];
            BString result;

            try
            {
                var bytes = strNode.Bytes.AsSpan().Slice(0, length);
                var str = Encoding.UTF8.GetString(strNode.Bytes.AsSpan().Slice(0, length));
                result = new BString(bytes.ToArray(), length, str);
            }
            catch (Exception e)
            {
                throw ConverterException.Create(
                    $"{nameof(BStringConverter)} - An error occurred while converting Bencode Abstract Sytax Tree.");
            }
            finally
            {
                BytePool.Pool.Return(strNode.Bytes, true);
            }

            return result;
        }
    }
}