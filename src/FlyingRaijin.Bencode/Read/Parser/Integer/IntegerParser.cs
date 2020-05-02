﻿using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using FlyingRaijin.Bencode.Read.Exceptions;
using System;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void IntegerParser(ParserContext context, NodeBase ast)
        {
            ////context.HasTokens();

            var node = new IntegerNode();
            ast.Children.Add(node);

            if (context.IsMatch(ZeroNode.Instance.Character))
            {
                ZeroParser(context, node);
                return;
            }

            if (context.IsMatch(NegativeSignNode.Instance.Character))
            {
                NegativeSignParser(context, node);
            }

            //if (DigitExcludingZeroNode.DigitsExcludingZero.Contains(context.LookAheadChar))
            if (context.IsMatch(DigitExcludingZeroNode.DigitsExcludingZero))
            {
                NumberParser(context, node);
            }
            else
            {
                throw ParsingException.Create("");
            }
        }
    }
}