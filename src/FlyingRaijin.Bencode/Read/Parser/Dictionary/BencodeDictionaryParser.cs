﻿using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Dictionary;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.List;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using FlyingRaijin.Bencode.Read.Ast.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void BencodeDictionaryParser(ParseContext context, NodeBase ast)
        {
            var node = new BencodeDictionaryNode();
            ast.Children.Add(node);

            var keys = new HashSet<BencodeStringNode>();

            DictionaryStartParser(context, node);
            ParseRecursiveDictionary(context, node, keys);
            EndParser(context, node);
        }

        private static void ParseRecursiveDictionary(ParseContext context, NodeBase ast, HashSet<BencodeStringNode> keys)
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
                    
                    BencodeIntegerParser(context, currentKeyValueNode);
                    node.Children.Add(currentKeyValueNode);
                    expectingKey = true;
                }
                else if (char.IsDigit(Convert.ToChar(context.LookAheadByte)))
                {
                    //- Possible Bencoded String
                    if (expectingKey)
                    {
                        currentKeyValueNode = new DictionaryKeyValueNode();

                        BencodeStringParser(context, currentKeyValueNode);

                        if (keys.Contains(currentKeyValueNode.Children.First()))
                        {
                            throw new Exception();
                        }

                        keys.Add((BencodeStringNode)currentKeyValueNode.Children.First());
                        expectingKey = false;
                    }
                    else
                    {
                        BencodeStringParser(context, currentKeyValueNode);
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
                    
                    BencodeListParser(context, currentKeyValueNode);
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

                    DictionaryStartParser(context, nestedDictionaryNode);
                    ParseRecursiveDictionary(context, nestedDictionaryNode, new HashSet<BencodeStringNode>());
                    EndParser(context, nestedDictionaryNode);

                    currentKeyValueNode.Children.Add(nestedDictionaryNode);
                    node.Children.Add(currentKeyValueNode);

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
