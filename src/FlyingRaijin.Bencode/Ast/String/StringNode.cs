using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.String
{
    public sealed class StringNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_STRING;
    }
}