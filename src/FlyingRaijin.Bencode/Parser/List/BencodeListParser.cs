using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Dictionary;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.List;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Dictionary;
using FlyingRaijin.Bencode.Parser.Integer;
using FlyingRaijin.Bencode.Parser.Shared;
using FlyingRaijin.Bencode.Parser.String;
using System;

namespace FlyingRaijin.Bencode.Parser.List
{
    public sealed class BencodeListParser : NonTerminalParserBase<BencodeListNode>
    {
        public static IParser Parser =>
            new BencodeListParser(Pack(
                    ListStartParser.Parser,
                    BencodeIntegerParser.Parser,
                    BencodeStringParser.Parser,
                    BencodeDictionaryParser.Parser,
                    EndParser.Parser));

        private BencodeListParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.BENCODED_LIST;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            var node = new BencodeListNode();
            ast.Children.Add(node);

            Parsers[Production.LIST_START].Parse(context, node);
            ParseRecursiveList(context, node);
            Parsers[Production.END].Parse(context, node);
        }

        private void ParseRecursiveList(ParseContext context, NodeBase ast)
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
                    Parsers[Production.BENCODED_INTEGER].Parse(context, node);
                }
                else if (char.IsDigit(Convert.ToChar(context.LookAheadByte)))
                {
                    //- Possible Bencoded String
                    Parsers[Production.BENCODED_STRING].Parse(context, node);
                }
                else if (context.LookAheadByte == ListStartNode.ListStartTerminalByte)
                {
                    //- Possible Bencoded List
                    var nestedListNode = new BencodeListNode();
                    node.Children.Add(nestedListNode);

                    Parsers[Production.LIST_START].Parse(context, nestedListNode);
                    ParseRecursiveList(context, nestedListNode);
                    Parsers[Production.END].Parse(context, nestedListNode);
                }
                else if (context.LookAheadByte == DictionaryStartNode.DictionaryStartTerminalByte)
                {
                    //- Possible Bencoded Dictionary
                    Parsers[Production.BENCODED_DICTIONARY].Parse(context, node);
                }
                else
                {
                    context.Match(context.LookAheadByte);
                }
            }
        }
    }
}