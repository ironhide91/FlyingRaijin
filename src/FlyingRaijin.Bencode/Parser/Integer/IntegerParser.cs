using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Exceptions;
using System;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void IntegerParser(ParseContext context, NodeBase ast)
        {
            context.HasTokens();

            var node = new IntegerNode();
            ast.Children.Add(node);

            if (context.LookAheadByte == '0')
            {
                ZeroParser(context, node);
                return;
            }

            if (context.LookAheadByte == '-')
            {
                NegativeSignParser(context, node);
            }

            if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadByte))
            {
                NumberParser(context, node);
            }
            else
            {
                throw ParsingException.Create(context.Position);
            }
        }
    }
}