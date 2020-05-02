using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Shared
{
    public sealed class EndNode : TerminalCharNodeBase
    {
        static EndNode()
        {
            Instance = new EndNode();
        }

        private EndNode()
        {

        }

        private static char UTF8Char = ToUTF8Char("e");

        public static readonly EndNode Instance;

        public override Production ProductionType => Production.END;

        public override char Character => UTF8Char;
    }
}