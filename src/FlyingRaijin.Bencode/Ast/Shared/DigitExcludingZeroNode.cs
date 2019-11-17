using FlyingRaijin.Bencode.Ast.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRaijin.Bencode.Ast.Shared
{
    public sealed class DigitExcludingZeroNode : TerminalByteNodeBase
    {
        public DigitExcludingZeroNode(byte nonZeroDigitByte)
        {
            if (DigitsExcludingZero.Contains(nonZeroDigitByte))
            {
                _NonTerminalCharacterByte = nonZeroDigitByte;
            }
            else
            {
                throw new Exception();
            }
        }

        private readonly byte _NonTerminalCharacterByte;

        public override Production ProductionType => Production.DIGIT_EXCULUDING_ZERO;

        public override byte Byte => _NonTerminalCharacterByte;        

        public static HashSet<byte> DigitsExcludingZero =>
            new HashSet<byte>(DigitsExcludingZeroChars.Select(x => Convert.ToByte(x)));

        private static char[] DigitsExcludingZeroChars => new char[]
        {
            '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
    }
}