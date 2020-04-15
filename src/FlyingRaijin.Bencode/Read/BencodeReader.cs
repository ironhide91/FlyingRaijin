using FlyingRaijin.Bencode.Read.Ast;
using FlyingRaijin.Bencode.Read.Ast.Dictionary;
using FlyingRaijin.Bencode.Read.Ast.Integer;
using FlyingRaijin.Bencode.Read.Ast.List;
using FlyingRaijin.Bencode.Read.Ast.String;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read.Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlyingRaijin.Bencode.Read
{
    public static class BencodeReader
    {
        private delegate IClrObject Reader(Encoding e, ParseContext c, TorrentRoot r);

        static BencodeReader()
        {
            var dict = new Dictionary<Type, Reader>
            {
                {
                    typeof(BInteger),
                    (encoding, context, root) =>
                    {
                        DelegateParsers.BencodeIntegerParser(context, root);
                        return BIntegerConverter.Converter.Convert(encoding, (BencodeIntegerNode)root.Children[0]);
                    }
                },

                {
                    typeof(BString),
                    (encoding, context, root) =>
                    {
                        DelegateParsers.BencodeStringParser(context, root);
                        return BStringConverter.Converter.Convert(encoding, (BencodeStringNode)root.Children[0]);
                    }
                },

                {
                    typeof(BList),
                    (encoding, context, root) =>
                    {
                        DelegateParsers.BencodeListParser(context, root);
                        return BListConverter.Converter.Convert(encoding, (BencodeListNode)root.Children[0]);
                    }
                },

                {
                    typeof(BDictionary),
                    (encoding, context, root) =>
                    {
                        DelegateParsers.BencodeDictionaryParser(context, root);
                        return BDictionaryConverter.Converter.Convert(encoding, (BencodeDictionaryNode)root.Children[0]);
                    }
                }
            };

            _Readers = new ReadOnlyDictionary<Type, Reader>(dict);
        }

        private static readonly IReadOnlyDictionary<Type, Reader> _Readers;

        public static T Read<T>(Encoding encoding, string bencodeValue) where T : struct, IClrObject
        {
            T bObject = default;

            using (var context = new ParseContext(encoding, bencodeValue))
            {
                var root = new TorrentRoot();

                bObject = (T)_Readers[typeof(T)](encoding, context, root);
            }          

            return bObject;
        }
    }
}