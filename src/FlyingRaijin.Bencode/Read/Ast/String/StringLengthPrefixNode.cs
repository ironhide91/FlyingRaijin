using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.String
{
    public sealed class StringLengthPrefixNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.STRING_LENGTH_PREFIX;

        public override byte Byte => LENGTH_PREFIX;

        public static byte LENGTH_PREFIX = ToByte(':');
    }
}