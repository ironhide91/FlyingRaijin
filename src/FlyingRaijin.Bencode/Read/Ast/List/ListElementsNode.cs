using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.List
{
    public sealed class ListElementsNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.LIST_ELEMENTS;
    }
}