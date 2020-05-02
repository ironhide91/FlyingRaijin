using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read.Converter;
using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class BIntegerConverter : IClrObjectConverter<BencodeIntegerNode, BInteger>
    {
        public static BIntegerConverter Converter => new BIntegerConverter();

        private BIntegerConverter()
        {

        }

        public BInteger Convert(BencodeIntegerNode node)
        {
            BInteger result;

            try
            {
                var numberNode = (IntegerNode)node.Children[1];

                var chars = new List<char>(numberNode.Children.Count);

                for (int i = 0; i < numberNode.Children.Count; i++)
                {
                    NodeBase current = numberNode.Children[i];

                    switch (current)
                    {
                        case NegativeSignNode n:
                            chars.Add(n.Character);
                            break;
                        case NumberNode n:
                            chars.AddRange(NumberConverter.Convert(n));
                            break;
                        case ZeroNode n:
                            chars.Add(n.Character);
                            break;
                        default:
                            break;
                    }
                }

                long value = 0;
                string intStr = null;

                try
                {
                    intStr = new string(chars.ToArray());

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