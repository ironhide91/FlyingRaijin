using FlyingRaijin.Bencode.Read.Ast.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRaijin.Bencode.Read.Ast.Shared
{
    public sealed class DigitExcludingZeroNode : TerminalCharNodeBase
    {
        private static string[] DigitsExcludingZeroChars => new string[]
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        public static HashSet<char> DigitsExcludingZero =>
            new HashSet<char>(DigitsExcludingZeroChars.Select(x => ToUTF8Char(x)));

        public DigitExcludingZeroNode(char nonZeroDigit)
        {
            if (DigitsExcludingZero.Contains(nonZeroDigit))
            {
                UTF8Char = nonZeroDigit;
            }
            else
            {
                throw new Exception();
            }
        }

        private readonly char UTF8Char;

        public override Production ProductionType => Production.DIGIT_EXCULUDING_ZERO;

        public override char Character => UTF8Char;        
    }
}