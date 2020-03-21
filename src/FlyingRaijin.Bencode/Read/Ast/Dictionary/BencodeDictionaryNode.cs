using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Dictionary
{
    public sealed class BencodeDictionaryNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_DICTIONARY;
    }
}