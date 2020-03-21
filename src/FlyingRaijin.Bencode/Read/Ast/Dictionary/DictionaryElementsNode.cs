using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Dictionary
{
    public sealed class DictionaryElementsNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.DICTIONARY_ELEMENTS;
    }
}