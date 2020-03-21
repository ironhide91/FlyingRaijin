using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FlyingRaijin.Bencode.Read.Ast.Base
{
    public abstract class TerminalByteNodeBase : NodeBase
    {
        public abstract byte Byte { get; }

        public override IList<NodeBase> Children { get => Empty; }

        private static ImmutableList<NodeBase> Empty => ImmutableList.Create<NodeBase>();

        public static byte ToByte(char character)
        {
            return Convert.ToByte(character);
        }
    }
}