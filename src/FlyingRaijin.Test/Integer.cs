using FlyingRaijin.Bencode.Ast;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Exceptions;
using FlyingRaijin.Bencode.Parser;
using System;
using System.Text;
using Xunit;

namespace FlyingRaijin.Bencode.Test
{
    public class Integer
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        
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
            var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);

            Assert.Equal(bnumber.Value, value);
        }

        [Fact]
        public void CanParseZero()
        {
            var bnumber = BencodeReader.Read<BInteger>(encoding, "i0e");

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
            var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            Assert.Equal(bnumber.Value, value);
        }

        [Theory]
        [InlineData("i9223372036854775807e", 9223372036854775807)]
        [InlineData("i-9223372036854775808e", -9223372036854775808)]
        public void CanParseInt64(string bencode, long value)
        {
            var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            Assert.Equal(bnumber.Value, value);
        }

        [Theory]
        [InlineData("i01e")]
        [InlineData("i012e")]
        [InlineData("i01234567890e")]
        [InlineData("i00001e")]
        public void LeadingZeros_ThrowsInvalidBencodeException(string bencode)
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
        }

        [Fact]
        public void MinusZero_ThrowsInvalidBencodeException()
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, "i-0e");
            });
        }

        [Theory]
        [InlineData("i")]
        [InlineData("i1")]
        [InlineData("i2")]
        [InlineData("i123")]
        public void MissingEndChar_ThrowsInvalidBencodeException(string bencode)
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
        }

        [Theory]
        [InlineData("1e")]
        [InlineData("42e")]
        [InlineData("100e")]
        [InlineData("1234567890e")]
        public void InvalidFirstChar_ThrowsInvalidBencodeException(string bencode)
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
        }

        [Fact]
        public void JustNegativeSign_ThrowsInvalidBencodeException()
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, "i-e");
            });
        }

        [Theory]
        [InlineData("i--1e")]
        [InlineData("i--42e")]
        [InlineData("i---100e")]
        [InlineData("i----1234567890e")]
        public void MoreThanOneNegativeSign_ThrowsInvalidBencodeException(string bencode)
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
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

            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
        }

        [Fact]
        public void BelowMinimumLength_ThrowsInvalidBencodeException()
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, "ie");
            });
        }

        [Theory]
        [InlineData("i9223372036854775808e")]
        [InlineData("i-9223372036854775809e")]
        public void LargerThanInt64_ThrowsUnsupportedException(string bencode)
        {
            Assert.Throws<ConverterException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
        }

        [Theory]
        [InlineData("i12345678901234567890e")]
        [InlineData("i123456789012345678901e")]
        [InlineData("i123456789012345678901234567890e")]
        public void LongerThanMaxDigits19_ThrowsUnsupportedException(string bencode)
        {
            Assert.Throws<ConverterException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(encoding, bencode);
            });
        }
    }
}