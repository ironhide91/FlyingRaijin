using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.String
{
    public sealed class CharNode : TerminalCharNodeBase
    {
        public CharNode(char character)
        {
            Char = character;
        }

        private readonly char Char;

        public override Production ProductionType => Production.CHAR;

        public override char Character => Char;
    }
}