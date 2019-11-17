using FlyingRaijin.Bencode.Ast;
using FlyingRaijin.Bencode.Ast.Integer;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Converter;
using FlyingRaijin.Bencode.Parser;
using FlyingRaijin.Bencode.Parser.Base;
using FlyingRaijin.Bencode.Parser.Integer;
using System;
using System.Text;
using System.Linq;
using System.IO;
using Xunit;

namespace FlyingRaijin.Bencode.Test
{
    public class Integer
    {
        private static Encoding encoding = Encoding.UTF8;

        private static IParser parser = BencodeIntegerParser.Parser;

        private static IClrObjectConverter<BencodeIntegerNode, BInteger> converter = BIntegerConverter.Converter;
        
        public Integer()
        {

        }

        [Theory]
        [InlineData("i1e", 1)]
        [InlineData("i2e", 2)]
        [InlineData("i3e", 3)]
        [InlineData("i42e", 42)]
        [InlineData("i100e", 100)]
        [InlineData("i1234567890e", 1234567890)]
        public void CanParsePositive(string bencode, int value)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            parser.Parse(context, root);            
            var bnumber = converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));

            Assert.Equal(bnumber.Value, value);
        }

        [Fact]
        public void CanParseZero()
        {
            var context = Helper.CreateParseContext("i0e");
            var root = new TorrentRoot();

            parser.Parse(context, root);
            var bnumber = converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));

            Assert.Equal(0, bnumber.Value);
        }

        [Theory]
        [InlineData("i-1e", -1)]
        [InlineData("i-2e", -2)]
        [InlineData("i-3e", -3)]
        [InlineData("i-42e", -42)]
        [InlineData("i-100e", -100)]
        [InlineData("i-1234567890e", -1234567890)]
        public void CanParseNegative(string bencode, int value)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            parser.Parse(context, root);
            var bnumber = converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));

            Assert.Equal(bnumber.Value, value);
        }

        [Theory]
        [InlineData("i9223372036854775807e", 9223372036854775807)]
        [InlineData("i-9223372036854775808e", -9223372036854775808)]
        public void CanParseInt64(string bencode, long value)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            parser.Parse(context, root);
            var bnumber = converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));

            Assert.Equal(bnumber.Value, value);
        }

        [Theory]
        [InlineData("i01e")]
        [InlineData("i012e")]
        [InlineData("i01234567890e")]
        [InlineData("i00001e")]
        public void LeadingZeros_ThrowsInvalidBencodeException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void MinusZero_ThrowsInvalidBencodeException()
        {
            var context = Helper.CreateParseContext("i-0e");
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData("i")]
        [InlineData("i1")]
        [InlineData("i2")]
        [InlineData("i123")]
        public void MissingEndChar_ThrowsInvalidBencodeException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<EndOfStreamException>(action);
        }

        [Theory]
        [InlineData("1e")]
        [InlineData("42e")]
        [InlineData("100e")]
        [InlineData("1234567890e")]
        public void InvalidFirstChar_ThrowsInvalidBencodeException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void JustNegativeSign_ThrowsInvalidBencodeException()
        {
            var context = Helper.CreateParseContext("i-e");
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData("i--1e")]
        [InlineData("i--42e")]
        [InlineData("i---100e")]
        [InlineData("i----1234567890e")]
        public void MoreThanOneNegativeSign_ThrowsInvalidBencodeException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData("i-e")]
        [InlineData("iasdfe")]
        [InlineData("i!#¤%&e")]
        [InlineData("i.e")]
        [InlineData("i42.e")]
        [InlineData("i42ae")]
        public void NonDigit_ThrowsInvalidBencodeException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void BelowMinimumLength_ThrowsInvalidBencodeException()
        {
            var context = Helper.CreateParseContext("ie");
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData("i9223372036854775808e")]
        [InlineData("i-9223372036854775809e")]
        public void LargerThanInt64_ThrowsUnsupportedException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData("i12345678901234567890e")]
        [InlineData("i123456789012345678901e")]
        [InlineData("i123456789012345678901234567890e")]
        public void LongerThanMaxDigits19_ThrowsUnsupportedException(string bencode)
        {
            var context = Helper.CreateParseContext(bencode);
            var root = new TorrentRoot();

            Action action = () =>
            {
                parser.Parse(context, root);
                converter.Convert(encoding, (BencodeIntegerNode)root.Children.ElementAt(0));
            };

            Assert.Throws<Exception>(action);
        }
    }
}