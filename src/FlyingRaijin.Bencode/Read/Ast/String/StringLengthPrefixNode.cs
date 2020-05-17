using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.String
{
    public sealed class StringLengthPrefixNode : TerminalCharNodeBase
    {
        static StringLengthPrefixNode()
        {
            Instance = new StringLengthPrefixNode();
        }

        private StringLengthPrefixNode()
        {

        }

        private static readonly char UTF8Char = ToUTF8Char(":");
        
        public static readonly StringLengthPrefixNode Instance;

        public override Production ProductionType => Production.STRING_LENGTH_PREFIX;

        public override char Character => UTF8Char;
    }
}