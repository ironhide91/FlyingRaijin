using FlyingRaijin.Bencode.Ast.Dictionary;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.Ast.List;
using FlyingRaijin.Bencode.Ast.String;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Converter;
using FlyingRaijin.Bencode.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlyingRaijin.Bencode.Parser
{
    public sealed class BDictionaryConverter : IClrObjectConverter<BencodeDictionaryNode, BDictionary>
    {
        public static BDictionaryConverter Converter => new BDictionaryConverter();

        private BDictionaryConverter()
        {

        }

        public BDictionary Convert(Encoding encoding, BencodeDictionaryNode node)
        {
            BDictionary result;

            try
            {
                var dictionary = new Dictionary<string, IClrObject>();

                foreach (var kv in node.Children[1].Children)
                {
                    var key = BStringConverter.Converter.Convert(encoding, (BencodeStringNode)kv.Children[0]).Value;

                    var value = kv.Children[1];

                    switch (value)
                    {
                        case BencodeIntegerNode n:
                            var bInteger = BIntegerConverter.Converter.Convert(encoding, n);
                            dictionary.Add(key, bInteger);
                            break;
                        case BencodeStringNode n:
                            var bString = BStringConverter.Converter.Convert(encoding, n);
                            dictionary.Add(key, bString);
                            break;
                        case BencodeListNode n:
                            var bList = BListConverter.Converter.Convert(encoding, n);
                            dictionary.Add(key, bList);
                            break;
                        case BencodeDictionaryNode n:
                            var bDictionary = Convert(encoding, n);
                            dictionary.Add(key, bDictionary);
                            break;
                        default:
                            break;
                    }
                }

                result = new BDictionary(new ReadOnlyDictionary<string, IClrObject>(dictionary));
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