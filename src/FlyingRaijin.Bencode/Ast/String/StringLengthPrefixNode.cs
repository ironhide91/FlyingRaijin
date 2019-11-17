using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Ast.String
{
    public sealed class StringLengthPrefixNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.STRING_LENGTH_PREFIX;

        public override byte Byte => LENGTH_PREFIX;

        public static byte LENGTH_PREFIX = ToByte(':');
    }
}