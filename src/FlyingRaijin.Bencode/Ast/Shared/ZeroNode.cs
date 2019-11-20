using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.Shared
{
    public sealed class ZeroNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.ZERO;

        public override byte Byte => ZeroDigitByte;

        public static byte ZeroDigitByte = ToByte('0');
    }
}