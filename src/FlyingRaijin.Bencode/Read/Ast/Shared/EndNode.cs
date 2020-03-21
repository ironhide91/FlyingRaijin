using FlyingRaijin.Bencode.Read.Ast.Base;
using System;

namespace FlyingRaijin.Bencode.Read.Ast.Shared
{
    public sealed class EndNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.END;

        public override byte Byte => IntegerEndNonTerminalByte;

        public static byte IntegerEndNonTerminalByte = ToByte('e');
    }
}