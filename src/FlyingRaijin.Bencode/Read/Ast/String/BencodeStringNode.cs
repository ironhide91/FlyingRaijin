using FlyingRaijin.Bencode.Read.Ast.Base;

namespace FlyingRaijin.Bencode.Read.Ast.String
{
    /*
     *  BSTRING => (ZERO | NUMBER) : (NIL | STRING)

        NUMBER => DIGIT_EXCULUDING_ZERO | { NUMBER | ZERO }        

        ZER0 => "0";

        DIGIT_EXCULUDING_ZERO => "1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9"

        STRING => CHARACTER | { STRING }

        CHARACTER => UTF-8 Character Set
     */

    public sealed class BencodeStringNode : NonTerminalNodeBase
    {
        public override Production ProductionType => Production.BENCODED_STRING;
    }
}