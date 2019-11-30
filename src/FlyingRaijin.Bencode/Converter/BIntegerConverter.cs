using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Converter;
using FlyingRaijin.Bencode.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Parser
{
    public sealed class BIntegerConverter : IClrObjectConverter<BencodeIntegerNode, BInteger>
    {
        public static BIntegerConverter Converter => new BIntegerConverter();

        private BIntegerConverter()
        {

        }

        public BInteger Convert(Encoding encoding, BencodeIntegerNode node)
        {
            BInteger result;

            try
            {
                var numberNode = (IntegerNode)node.Children[1];

                var bytes = new List<byte>(numberNode.Children.Count);

                for (int i = 0; i < numberNode.Children.Count; i++)
                {
                    NodeBase current = numberNode.Children[i];

                    switch (current)
                    {
                        case NegativeSignNode n:
                            bytes.Add(n.Byte);
                            break;
                        case NumberNode n:
                            bytes.AddRange(NumberConverter.Convert(n));
                            break;
                        case ZeroNode n:
                            bytes.Add(n.Byte);
                            break;
                        default:
                            break;
                    }
                }

                long value = 0;
                string intStr = null;

                try
                {
                    intStr = encoding.GetString(bytes.ToArray());

                    checked
                    {
                        value = long.Parse(intStr);
                    }
                }
                catch (OverflowException)
                {
                    throw ConverterException.Create($"Integer Overflow exception while parsing {intStr} into long type.");
                }

                result = new BInteger(value);
            }
            catch (ConverterException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw ConverterException.Create(
                        $"{nameof(BIntegerConverter)} - An error occurred while converting Bencode Abstract Sytax Tree.");
            }

            return result;
        }
    }
}