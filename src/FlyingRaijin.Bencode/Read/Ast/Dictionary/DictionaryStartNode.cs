using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Dictionary
{
    public sealed class DictionaryStartNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.DICTIONARY_START;

        public override byte Byte => DictionaryStartTerminalByte;

        public static byte DictionaryStartTerminalByte = ToByte('d');
    }
}