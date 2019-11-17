﻿using FlyingRaijin.Bencode.Ast;
using FlyingRaijin.Bencode.Ast.Dictionary;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Converter;
using FlyingRaijin.Bencode.Parser;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Dictionary;
using System.Linq;
using System.Text;
using Xunit;

namespace FlyingRaijin.Bencode.Test
{
    public class Dictionary
    {
        private static Encoding encoding = Encoding.UTF8;

        private static IParser parser = BencodeDictionaryParser.Parser;

        private static IClrObjectConverter<BencodeDictionaryNode, BDictionary> converter = BDictionaryConverter.Converter;

        [Theory]
        [InlineData("de")]
        public void CanParseEmpty(string bencode)
        {
            //- Arrange
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            //- Act
            parser.Parse(context, root);
            var bDictionary = converter.Convert(encoding, (BencodeDictionaryNode)root.Children.ElementAt(0));

            //-Assert
            Assert.Equal(0, bDictionary.Value.Count);
        }

        [Theory]
        [InlineData("d4:spam3:egge")]
        public void Case1(string bencode)
        {
            //- Arrange
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            //- Act
            parser.Parse(context, root);
            var bDictionary = converter.Convert(encoding, (BencodeDictionaryNode)root.Children.ElementAt(0));

            //-Assert
            Assert.Equal(1, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));

            var value = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value.Length);
            Assert.Equal("egg", value.Value);
        }


        [Theory]
        [InlineData("d4:spam3:egg3:cow3:mooe")]
        public void Case2(string bencode)
        {
            //- Arrange
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            //- Act
            parser.Parse(context, root);
            var bDictionary = converter.Convert(encoding, (BencodeDictionaryNode)root.Children.ElementAt(0));

            //- Assert
            Assert.Equal(2, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));
            Assert.True(bDictionary.Value.ContainsKey("cow"));

            var value1 = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value1.Length);
            Assert.Equal("egg", value1.Value);

            var value2 = (BString)bDictionary.Value["cow"];
            Assert.Equal(3, value2.Length);
            Assert.Equal("moo", value2.Value);
        }

        [Theory]
        [InlineData("d4:spam3:egg3:cow3:moo3:inti99e6:numberi753ee")]
        public void Case3(string bencode)
        {
            //- Arrange
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            //- Act
            parser.Parse(context, root);
            var bDictionary = converter.Convert(encoding, (BencodeDictionaryNode)root.Children.ElementAt(0));

            //- Assert
            Assert.Equal(4, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));
            Assert.True(bDictionary.Value.ContainsKey("cow"));
            Assert.True(bDictionary.Value.ContainsKey("int"));
            Assert.True(bDictionary.Value.ContainsKey("number"));

            var value1 = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value1.Length);
            Assert.Equal("egg", value1.Value);

            var value2 = (BString)bDictionary.Value["cow"];
            Assert.Equal(3, value2.Length);
            Assert.Equal("moo", value2.Value);

            var value3 = (BInteger)bDictionary.Value["int"];
            Assert.Equal(99L, value3.Value);

            var value4 = (BInteger)bDictionary.Value["number"];
            Assert.Equal(753L, value4.Value);
        }

        [Theory]
        [InlineData("d4:spam3:egg3:cow3:moo3:inti99e6:numberi753e4:listl5:rahul5:bipini123456789eee")]
        public void Case4(string bencode)
        {
            //- Arrange
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            //- Act
            parser.Parse(context, root);
            var bDictionary = converter.Convert(encoding, (BencodeDictionaryNode)root.Children.ElementAt(0));

            //- Assert
            Assert.Equal(5, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));
            Assert.True(bDictionary.Value.ContainsKey("cow"));
            Assert.True(bDictionary.Value.ContainsKey("int"));
            Assert.True(bDictionary.Value.ContainsKey("number"));
            Assert.True(bDictionary.Value.ContainsKey("list"));

            var value1 = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value1.Length);
            Assert.Equal("egg", value1.Value);

            var value2 = (BString)bDictionary.Value["cow"];
            Assert.Equal(3, value2.Length);
            Assert.Equal("moo", value2.Value);

            var value3 = (BInteger)bDictionary.Value["int"];
            Assert.Equal(99L, value3.Value);

            var value4 = (BInteger)bDictionary.Value["number"];
            Assert.Equal(753L, value4.Value);

            var value5 = (BList)bDictionary.Value["list"];
            Assert.Equal(3, value5.Value.Count);

            var value50 = (BString)value5.Value.ElementAt(0);
            Assert.Equal(5, value50.Length);
            Assert.Equal("rahul", value50.Value);

            var value51 = (BString)value5.Value.ElementAt(1);
            Assert.Equal(5, value51.Length);
            Assert.Equal("bipin", value51.Value);

            var value52 = (BInteger)value5.Value.ElementAt(2);
            Assert.Equal(123456789L, value52.Value);
        }
    }
}
