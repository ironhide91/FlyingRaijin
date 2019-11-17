using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Integer
{
    public sealed class IntegerNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.INTEGER;
    }
}