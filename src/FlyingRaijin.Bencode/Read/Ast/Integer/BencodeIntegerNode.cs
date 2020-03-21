using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Integer
{
    public sealed class BencodeIntegerNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_INTEGER;
    }
}