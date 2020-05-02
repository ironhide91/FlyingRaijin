using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Shared
{
    public sealed class ZeroNode : TerminalCharNodeBase
    {
        static ZeroNode()
        {
            Instance = new ZeroNode();
        }

        private ZeroNode()
        {

        }

        private static char UTF8Char = ToUTF8Char("0");

        public static readonly ZeroNode Instance;

        public override Production ProductionType => Production.ZERO;

        public override char Character => UTF8Char;
    }
}