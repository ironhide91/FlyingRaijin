using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Dictionary;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.List;
using FlyingRaijin.Bencode.Ast.Shared;
using System;

namespace FlyingRaijin.Bencode.Parser
{
    public static partial class DelegateParsers
    {
        public static void BencodeListParser(ParseContext context, NodeBase ast)
        {
            var node = new BencodeListNode();
            ast.Children.Add(node);

            ListStartParser(context, node);
            ParseRecursiveList(context, node);
            EndParser(context, node);
        }

        private static void ParseRecursiveList(ParseContext context, NodeBase ast)
        {
            context.HasTokens();

            var node = new ListElementsNode();
            ast.Children.Add(node);

            while (true)
            {
                if (context.LookAheadByte == EndNode.IntegerEndNonTerminalByte)
                {
                    break;
                }

                if (context.LookAheadByte == IntegerStartNode.IntegerStartNonTerminalByte)
                {
                    //- Possible Bencoded Integer
                    BencodeIntegerParser(context, node);
                }
                else if (char.IsDigit(Convert.ToChar(context.LookAheadByte)))
                {
                    //- Possible Bencoded String
                    BencodeStringParser(context, node);
                }
                else if (context.LookAheadByte == ListStartNode.ListStartTerminalByte)
                {
                    //- Possible Bencoded List
                    var nestedListNode = new BencodeListNode();
                    node.Children.Add(nestedListNode);

                    ListStartParser(context, nestedListNode);
                    ParseRecursiveList(context, nestedListNode);
                    EndParser(context, nestedListNode);
                }
                else if (context.LookAheadByte == DictionaryStartNode.DictionaryStartTerminalByte)
                {
                    //- Possible Bencoded Dictionary
                    BencodeDictionaryParser(context, node);
                }
                else
                {
                    context.Match(context.LookAheadByte);
                }
            }
        }
    }
}