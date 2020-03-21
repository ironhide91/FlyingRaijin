using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.List
{
    public sealed class ListStartNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.LIST_START;

        public override byte Byte => ListStartTerminalByte;

        public static byte ListStartTerminalByte = ToByte('l');
    }
}