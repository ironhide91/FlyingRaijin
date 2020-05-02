using FlyingRaijin.Bencode.Read.Ast.Dictionary;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.List;
using FlyingRaijin.Bencode.Read.Ast.String;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read.Converter;
using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class BDictionaryConverter : IClrObjectConverter<BencodeDictionaryNode, BDictionary>
    {
        public static BDictionaryConverter Converter => new BDictionaryConverter();

        private BDictionaryConverter()
        {

        }

        public BDictionary Convert(BencodeDictionaryNode node)
        {
            BDictionary result;

            try
            {
                var dictionary = new Dictionary<string, IClrObject>();

                foreach (var kv in node.Children[1].Children)
                {
                    var key = BStringConverter.Converter.Convert((BencodeStringNode)kv.Children[0]).Value;

                    var value = kv.Children[1];

                    switch (value)
                    {
                        case BencodeIntegerNode n:
                            var bInteger = BIntegerConverter.Converter.Convert( n);
                            dictionary.Add(key, bInteger);
                            break;
                        case BencodeStringNode n:
                            var bString = BStringConverter.Converter.Convert(n);
                            dictionary.Add(key, bString);
                            break;
                        case BencodeListNode n:
                            var bList = BListConverter.Converter.Convert(n);
                            dictionary.Add(key, bList);
                            break;
                        case BencodeDictionaryNode n:
                            var bDictionary = Convert(n);
                            dictionary.Add(key, bDictionary);
                            break;
                        default:
                            break;
                    }
                }

                result = new BDictionary(ImmutableDictionary.CreateRange(dictionary));
            }
            catch (Exception e)
            {
                throw ConverterException.Create(
                        $"{nameof(BDictionaryConverter)} - An error occurred while converting Bencode Abstract Sytax Tree.");
            }

            return result;
        }
    }
}