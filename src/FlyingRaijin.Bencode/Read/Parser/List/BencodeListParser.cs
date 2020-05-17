using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Dictionary;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.List;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using System;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void BencodeListParser(ParserContext context, NodeBase ast)
        {
            var node = new BencodeListNode();
            ast.Children.Add(node);

            ListStartParser(context, node);
            ParseRecursiveList(context, node);
            EndParser(context, node);
        }

        private static void ParseRecursiveList(ParserContext context, NodeBase ast)
        {
            var node = new ListElementsNode();
            ast.Children.Add(node);

            while (true)
            {
                if (context.LookAheadChar == EndNode.Instance.Character)
                {
                    break;
                }

                if (context.LookAheadChar == IntegerStartNode.Instance.Character)
                {
                    //- Possible Bencoded Integer
                    BencodeIntegerParser(context, node);
                }
                else if (char.IsDigit(Convert.ToChar(context.LookAheadChar)))
                {
                    //- Possible Bencoded String
                    BencodeStringParser(context, node);
                }
                else if (context.LookAheadChar == ListStartNode.Instance.Character)
                {
                    //- Possible Bencoded List
                    var nestedListNode = new BencodeListNode();
                    node.Children.Add(nestedListNode);

                    ListStartParser(context, nestedListNode);
                    ParseRecursiveList(context, nestedListNode);
                    EndParser(context, nestedListNode);
                }
                else if (context.LookAheadChar == DictionaryStartNode.Instance.Character)
                {
                    //- Possible Bencoded Dictionary
                    BencodeDictionaryParser(context, node);
                }
                else
                {
                    context.Match(context.LookAheadChar);
                }
            }
        }
    }
}