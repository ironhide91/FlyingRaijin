using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Dictionary
{
    public sealed class DictionaryStartNode : TerminalCharNodeBase
    {
        static DictionaryStartNode()
        {
            Instance = new DictionaryStartNode();
        }

        private DictionaryStartNode()
        {

        }

        private static char UTF8Char = ToUTF8Char("d");

        public static readonly DictionaryStartNode Instance;

        public override Production ProductionType => Production.DICTIONARY_START;

        public override char Character => UTF8Char;
    }
}