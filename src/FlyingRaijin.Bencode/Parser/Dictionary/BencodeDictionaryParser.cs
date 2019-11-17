using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Dictionary;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.List;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Integer;
using FlyingRaijin.Bencode.Parser.List;
using FlyingRaijin.Bencode.Parser.Shared;
using FlyingRaijin.Bencode.Parser.String;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Parser.Dictionary
{
    public sealed class BencodeDictionaryParser : NonTerminalParserBase<BencodeListNode>
    {
        public static IParser Parser =>
            new BencodeDictionaryParser(Pack(
                    DictionaryStartParser.Parser,
                    BencodeIntegerParser.Parser,
                    BencodeStringParser.Parser,
                    BencodeListParser.Parser,
                    EndParser.Parser));

        private BencodeDictionaryParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.BENCODED_LIST;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            var node = new BencodeDictionaryNode();
            ast.Children.Add(node);

            var keys = new HashSet<BencodeStringNode>();

            Parsers[Production.DICTIONARY_START].Parse(context, node);
            ParseRecursiveDictionary(context, node, keys);
            Parsers[Production.END].Parse(context, node);
        }

        private void ParseRecursiveDictionary(ParseContext context, NodeBase ast, HashSet<BencodeStringNode> keys)
        {
            context.HasTokens();

            var node = new DictionaryElementsNode();
            ast.Children.Add(node);

            DictionaryKeyValueNode currentKeyValueNode = null;

            bool expectingKey = true;

            while (true)
            {
                if (context.LookAheadByte == EndNode.IntegerEndNonTerminalByte)
                {
                    break;
                }

                if (context.LookAheadByte == IntegerStartNode.IntegerStartNonTerminalByte)
                {
                    //- Possible Bencoded Integer
                    if (expectingKey)
                    {
                        throw new Exception();
                    }
                    
                    Parsers[Production.BENCODED_INTEGER].Parse(context, currentKeyValueNode);
                    node.Children.Add(currentKeyValueNode);
                    expectingKey = true;
                }
                else if (char.IsDigit(Convert.ToChar(context.LookAheadByte)))
                {
                    //- Possible Bencoded String
                    if (expectingKey)
                    {
                        currentKeyValueNode = new DictionaryKeyValueNode();

                        Parsers[Production.BENCODED_STRING].Parse(context, currentKeyValueNode);

                        if (keys.Contains(currentKeyValueNode.Children.First()))
                        {
                            throw new Exception();
                        }

                        keys.Add((BencodeStringNode)currentKeyValueNode.Children.First());
                        expectingKey = false;
                    }
                    else
                    {
                        Parsers[Production.BENCODED_STRING].Parse(context, currentKeyValueNode);
                        node.Children.Add(currentKeyValueNode);
                        expectingKey = true;
                    }
                }
                else if (context.LookAheadByte == ListStartNode.ListStartTerminalByte)
                {
                    //- Possible Bencoded List
                    if (expectingKey)
                    {
                        throw new Exception();
                    }
                    
                    Parsers[Production.BENCODED_LIST].Parse(context, currentKeyValueNode);
                    node.Children.Add(currentKeyValueNode);
                    expectingKey = true;
                }
                else if (context.LookAheadByte == DictionaryStartNode.DictionaryStartTerminalByte)
                {
                    if (expectingKey)
                    {
                        throw new Exception();
                    }

                    //- Possible Bencoded Dictionary
                    var nestedDictionaryNode = new BencodeDictionaryNode();
                    node.Children.Add(nestedDictionaryNode);

                    Parsers[Production.DICTIONARY_START].Parse(context, nestedDictionaryNode);
                    //ParseRecursiveDictionary(context, nestedDictionaryNode);
                    Parsers[Production.END].Parse(context, nestedDictionaryNode);

                    expectingKey = true;
                }
                else
                {
                    context.Match(context.LookAheadByte);
                }
            }
        }
    }
}