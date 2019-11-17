using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BencodingLibrary.Parser
{

    public interface IClrObject
    {

    }

    public interface IClrObject<T> : IClrObject
    {
        T Value { get; }
    }

    public sealed class BInteger : IClrObject<long>
    {
        public BInteger(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }
    }

    public sealed class BString : IClrObject<string>
    {
        public BString(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    public sealed class BList<T> : IClrObject<IReadOnlyList<IClrObject<T>>>
    {
        public BList(IReadOnlyList<IClrObject<T>> value)
        {
            Value = value;
        }

        public IReadOnlyList<IClrObject<T>> Value { get; private set; }
    }

    public sealed class BDictionary<T> : IClrObject<IReadOnlyDictionary<BString, IClrObject<T>>>
    {
        public BDictionary(IReadOnlyDictionary<BString, IClrObject<T>> value)
        {
            Value = value;
        }

        public IReadOnlyDictionary<BString, IClrObject<T>> Value { get; private set; }
    }

    public interface IClrObjectConverter<N, T>
        where N : NonTerminalNodeBase
        where T : IClrObject
    {
        T Convert(N node);
    }

    public sealed class BIntegerConverter : IClrObjectConverter<BencodeIntegerNode, BInteger>
    {
        public BInteger Convert(BencodeIntegerNode node)
        {
            var numberNode = (IntegerNode)node.Children[1];

            var characters = new StringBuilder();

            for (int i = 0; i < numberNode.Children.Count; i++)
            {
                NodeBase current = numberNode.Children[i];

                switch (current)
                {
                    case NegativeSignNode n:
                        characters.Append(n.NonTerminalCharacter);
                        break;
                    case NumberNode n:
                        foreach (TerminalNodeBase item in n.Children)
                        {
                            characters.Append(item.NonTerminalCharacter);
                        }
                        break;
                    case ZeroNode n:
                        characters.Append(n.NonTerminalCharacter);
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
                    value = long.Parse(characters.ToString());
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