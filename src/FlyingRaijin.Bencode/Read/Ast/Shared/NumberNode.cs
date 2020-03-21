using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Shared
{
    public sealed class NumberNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.NUMBER;
    }
}