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
using System.IO;
using System.Text;

namespace FlyingRaijin.Bencode.Read
{
    public static class BencodeReader
    {
        private delegate IClrObject Reader(ParserContext c, TorrentRoot r);

        static BencodeReader()
        {
            var dict = new Dictionary<Type, Reader>
            {
                {
                    typeof(BInteger),
                    (context, root) =>
                    {
                        DelegateParsers.BencodeIntegerParser(context, root);
                        return BIntegerConverter.Converter.Convert((BencodeIntegerNode)root.Children[0]);
                    }
                },

                {
                    typeof(BString),
                    (context, root) =>
                    {
                        DelegateParsers.BencodeStringParser(context, root);
                        return BStringConverter.Converter.Convert((BencodeStringNode)root.Children[0]);
                    }
                },

                {
                    typeof(BList),
                    (context, root) =>
                    {
                        DelegateParsers.BencodeListParser(context, root);
                        return BListConverter.Converter.Convert((BencodeListNode)root.Children[0]);
                    }
                },

                {
                    typeof(BDictionary),
                    (context, root) =>
                    {
                        DelegateParsers.BencodeDictionaryParser(context, root);
                        return BDictionaryConverter.Converter.Convert((BencodeDictionaryNode)root.Children[0]);
                    }
                }
            };

            _Readers = new ReadOnlyDictionary<Type, Reader>(dict);
        }

        private static readonly IReadOnlyDictionary<Type, Reader> _Readers;

        public static T Read<T>(string bencodeValue) where T : struct, IClrObject
        {
            T bObject = default;

            using (var context = new ParserContext(bencodeValue))
            {
                var root = new TorrentRoot();

                bObject = (T)_Readers[typeof(T)](context, root);
            }          

            return bObject;
        }

        public static T Read<T>(Stream stream) where T : struct, IClrObject
        {
            T bObject = default;

            using (var context = new ParserContext(stream))
            {
                var root = new TorrentRoot();

                bObject = (T)_Readers[typeof(T)](context, root);
            }

            return bObject;
        }
    }
}