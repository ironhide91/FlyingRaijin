using FlyingRaijin.Bencode.Read.Ast.Base;
using System;

namespace FlyingRaijin.Bencode.Read.Ast.Integer
{
    public sealed class IntegerStartNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.INTEGER_START;

        public override byte Byte => IntegerStartNonTerminalByte;

        public static byte IntegerStartNonTerminalByte = ToByte('i');
    }
}