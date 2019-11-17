using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Dictionary
{
    public sealed class DictionaryElementsNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.DICTIONARY_ELEMENTS;
    }
}