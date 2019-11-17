using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.List
{
    public sealed class BencodeListNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_LIST;
    }
}