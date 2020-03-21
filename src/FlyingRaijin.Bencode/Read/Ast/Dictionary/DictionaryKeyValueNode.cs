using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Dictionary
{
    public sealed class DictionaryKeyValueNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.DICTIONARY_KEY_VALUE;
    }
}