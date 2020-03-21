using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.Integer
{
    public sealed class NegativeSignNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.INTEGER_START;

        public override byte Byte => NegativeSignByte;

        public static byte NegativeSignByte = ToByte('-');
    }
}