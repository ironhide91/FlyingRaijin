using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRaijin.Bencode.Ast.Base
{
    public abstract class TerminalByteNodeBase : NodeBase
    {
        public abstract byte Byte { get; }

        public override ICollection<NodeBase> Children { get => Empty; }

        private static ICollection<NodeBase> Empty => Enumerable.Empty<NodeBase>().ToList().AsReadOnly();

        public static byte ToByte(char character)
        {
            return Convert.ToByte(character);
        }
    }
}