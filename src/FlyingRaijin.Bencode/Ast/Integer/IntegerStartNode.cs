using FlyingRaijin.Bencode.Ast.Base;
using System;

namespace FlyingRaijin.Bencode.Ast.Integer
{
    public sealed class IntegerStartNode : TerminalByteNodeBase
    {
        public override Production ProductionType => Production.INTEGER_START;

        public override byte Byte => IntegerStartNonTerminalByte;

        public static byte IntegerStartNonTerminalByte = ToByte('i');
    }
}