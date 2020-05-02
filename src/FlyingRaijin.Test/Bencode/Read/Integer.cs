using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read.Exceptions;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class Integer
    {
        [Theory]
        [InlineData("i1e", 1)]
        [InlineData("i2e", 2)]
        [InlineData("i3e", 3)]
        [InlineData("i42e", 42)]
        [InlineData("i100e", 100)]
        [InlineData("i1234567890e", 1234567890)]
        public void CanParsePositive(string bencode, int value)
        {
            var bnumber = BencodeReader.Read<BInteger>(bencode);            

            Assert.Equal(bnumber.Value, value);
        }

        [Fact]
        public void CanParseZero()
        {
            var bnumber = BencodeReader.Read<BInteger>("i0e");

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
            var bnumber = BencodeReader.Read<BInteger>(bencode);
            Assert.Equal(bnumber.Value, value);
        }

        [Theory]
        [InlineData("i9223372036854775807e", 9223372036854775807)]
        [InlineData("i-9223372036854775808e", -9223372036854775808)]
        public void CanParseInt64(string bencode, long value)
        {
            var bnumber = BencodeReader.Read<BInteger>(bencode);
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
                BencodeReader.Read<BInteger>(bencode);
            });
        }

        [Fact]
        public void MinusZero_ThrowsInvalidBencodeException()
        {
            Assert.Throws<ParsingException>(() =>
            {
                BencodeReader.Read<BInteger>("i-0e");
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
                BencodeReader.Read<BInteger>(bencode);
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
                BencodeReader.Read<BInteger>(bencode);
            });
        }

        [Fact]
        public void JustNegativeSign_ThrowsInvalidBencodeException()
        {
            Assert.Throws<ParsingException>(() =>
            {
                BencodeReader.Read<BInteger>("i-e");
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
                BencodeReader.Read<BInteger>(bencode);
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
                var bnumber = BencodeReader.Read<BInteger>(bencode);
            });
        }

        [Fact]
        public void BelowMinimumLength_ThrowsInvalidBencodeException()
        {
            Assert.Throws<ParsingException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>("ie");
            });
        }

        [Theory]
        [InlineData("i9223372036854775808e")]
        [InlineData("i-9223372036854775809e")]
        public void LargerThanInt64_ThrowsUnsupportedException(string bencode)
        {
            Assert.Throws<ConverterException>(() =>
            {
                var bnumber = BencodeReader.Read<BInteger>(bencode);
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
                var bnumber = BencodeReader.Read<BInteger>(bencode);
            });
        }
    }
}