using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.List
{
    public sealed class BencodeListNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_LIST;
    }
}