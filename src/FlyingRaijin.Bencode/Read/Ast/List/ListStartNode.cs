using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.List
{
    public sealed class ListStartNode : TerminalCharNodeBase
    {
        static ListStartNode()
        {
            Instance = new ListStartNode();
        }

        private ListStartNode()
        {

        }

        private static char UTF8Char = ToUTF8Char("l"); 
        
        public static readonly ListStartNode Instance;

        public override Production ProductionType => Production.LIST_START;

        public override char Character => UTF8Char;        
    }
}