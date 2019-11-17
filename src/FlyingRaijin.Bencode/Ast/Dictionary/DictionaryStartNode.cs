using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Dictionary
{
    public sealed class DictionaryStartNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.DICTIONARY_START;

        public override byte Byte => DictionaryStartTerminalByte;

        public static byte DictionaryStartTerminalByte = ToByte('d');
    }
}