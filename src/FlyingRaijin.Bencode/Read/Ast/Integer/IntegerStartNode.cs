using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Integer
{
    public sealed class IntegerStartNode : TerminalCharNodeBase
    {
        static IntegerStartNode()
        {
            Instance = new IntegerStartNode();
        }

        private IntegerStartNode()
        {

        }

        public static readonly IntegerStartNode Instance;

        public override Production ProductionType => Production.INTEGER_START;

        public override char Character => UTF8Char;

        private static char UTF8Char = ToUTF8Char("i");
    }
}