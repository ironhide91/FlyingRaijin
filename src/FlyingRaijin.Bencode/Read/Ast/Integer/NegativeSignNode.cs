using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Integer
{
    public sealed class NegativeSignNode : TerminalCharNodeBase
    {
        static NegativeSignNode()
        {
            Instance = new NegativeSignNode();
        }

        private NegativeSignNode()
        {

        }

        private static char UTF8Char = ToUTF8Char("-");
        
        public static readonly NegativeSignNode Instance;

        public override Production ProductionType => Production.INTEGER_START;

        public override char Character => UTF8Char;
    }
}