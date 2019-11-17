using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.List
{
    public sealed class ListElementsNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.LIST_ELEMENTS;
    }
}