using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Integer
{
    public sealed class IntegerNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.INTEGER;
    }
}