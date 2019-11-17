using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Shared;
using FlyingRaijin.Bencode.Converter;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace FlyingRaijin.Bencode.Parser.String
{
    public sealed class BencodeStringParser : NonTerminalParserBase<BencodeStringNode>
    {
        public static BencodeStringParser Parser =>
            new BencodeStringParser(Pack(NumberParser.Parser, StringLengthPrefixParser.Parser, StringParser.Parser));

        private BencodeStringParser(ParserDictionary dependentParsers) : base(dependentParsers)
        {
            Parsers = dependentParsers;
        }

        public override Production ProductionType => Production.BENCODED_STRING;

        protected override ParserDictionary Parsers { get; set; }

        public override void Parse(ParseContext context, NodeBase ast)
        {
            var node = new BencodeStringNode();
            ast.Children.Add(node);

            Parsers[Production.NUMBER].Parse(context, node);
            Parsers[Production.STRING_LENGTH_PREFIX].Parse(context, node);

            var lengthBytes = NumberConverter.Convert(node.Children.ElementAt(0));
            var length = int.Parse(Encoding.UTF8.GetString(lengthBytes));

            ((IStringParser)Parsers[Production.STRING]).BytesToProcess = length;
            Parsers[Production.STRING].Parse(context, node);
        }
    }
}