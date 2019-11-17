using FlyingRaijin.Bencode.Ast.Base;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRaijin.Bencode.Ast.String
{
    public sealed class ByteNode : NodeBase
    {
        public ByteNode(byte charByte)
        {
            Byte = charByte;
        }

        public readonly byte Byte;

        public override ICollection<NodeBase> Children { get => Empty; }

        private static ICollection<NodeBase> Empty => Enumerable.Empty<NodeBase>().ToList().AsReadOnly();

        public override Production ProductionType => Production.BYTE;
    }
}