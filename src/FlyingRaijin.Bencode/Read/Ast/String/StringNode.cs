using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.String
{
    public sealed class StringNode : NonTerminalNodeBase
    {
        public StringNode(byte[] bytes)
        {
            Bytes = bytes;
        }

        public readonly byte[] Bytes;

        public override Production ProductionType => Production.BENCODED_STRING;
    }
}