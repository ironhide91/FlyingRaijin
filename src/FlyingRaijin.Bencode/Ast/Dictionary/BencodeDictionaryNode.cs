using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Dictionary
{
    public sealed class BencodeDictionaryNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_DICTIONARY;
    }
}