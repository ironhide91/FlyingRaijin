using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Dictionary
{
    public sealed class DictionaryKeyValueNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.DICTIONARY_KEY_VALUE;
    }
}