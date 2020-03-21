using FlyingRaijin.Bencode.Read.Ast.Dictionary;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.List;
using FlyingRaijin.Bencode.Read.Ast.String;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read.Converter;
using FlyingRaijin.Bencode.Read.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public sealed class BListConverter : IClrObjectConverter<BencodeListNode, BList>
    {
        public static BListConverter Converter => new BListConverter();

        private BListConverter()
        {

        }

        public BList Convert(Encoding encoding, BencodeListNode node)
        {
            BList result;

            try
            {
                var list = new List<IClrObject>();

                foreach (var item in node.Children[1].Children)
                {
                    switch (item)
                    {
                        case BencodeIntegerNode n:
                            var bInteger = BIntegerConverter.Converter.Convert(encoding, n);
                            list.Add(bInteger);
                            break;
                        case BencodeStringNode n:
                            var bString = BStringConverter.Converter.Convert(encoding, n);
                            list.Add(bString);
                            break;
                        case BencodeListNode n:
                            var bList = Convert(encoding, n);
                            list.Add(bList);
                            break;
                        case BencodeDictionaryNode n:
                            var bDictionary = BDictionaryConverter.Converter.Convert(encoding, n);
                            list.Add(bDictionary);
                            break;
                        default:
                            break;
                    }
                }

                result = new BList(new ReadOnlyCollection<IClrObject>(list));
            }
            catch (Exception e)
            {
                throw ConverterException.Create(
                        $"{nameof(BStringConverter)} - An error occurred while converting Bencode Abstract Sytax Tree.");
            }            

            return result;
        }
    }
}