using FlyingRaijin.Bencode.Ast.Base;
using System;

namespace FlyingRaijin.Bencode.Ast.Shared
{
    public sealed class EndNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.END;

        public override byte Byte => IntegerEndNonTerminalByte;

        public static byte IntegerEndNonTerminalByte = ToByte('e');
    }
}