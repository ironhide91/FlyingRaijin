using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Shared
{
    public sealed class NumberNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.NUMBER;
    }
}