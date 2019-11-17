using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Converter;
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
            var numberNode = (IntegerNode)node.Children.ElementAt(1);

            var bytes = new List<byte>(numberNode.Children.Count);

            for (int i = 0; i < numberNode.Children.Count; i++)
            {
                NodeBase current = numberNode.Children.ElementAt(i);

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

            try
            {
                checked
                {
                    value = long.Parse(encoding.GetString(bytes.ToArray()));
                }
            }
            catch (OverflowException)
            {
                throw new Exception("");
            }

            return new BInteger(value);
        }
    }
}