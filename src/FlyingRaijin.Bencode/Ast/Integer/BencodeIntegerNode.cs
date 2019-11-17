using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Integer
{
    public sealed class BencodeIntegerNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_INTEGER;
    }
}