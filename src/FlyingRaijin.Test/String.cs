using FluentAssertions;
using FlyingRaijin.Bencode.ClrObject;
using FlyingRaijin.Bencode.Exceptions;
using System;
using System.Text;
using Xunit;

namespace FlyingRaijin.Bencode.Test
{
    public class String
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        
        public String()
        {

        }

        [Theory]
        [InlineData("1:s")]
        [InlineData("4:spam")]
        [InlineData("8:spameggs")]
        [InlineData("9:spam eggs")]
        [InlineData("9:spam:eggs")]
        [InlineData("14:!@#₪%&/()=?$|")]
        public void CanParseSimple(string bencode)
        {
            var parts = bencode.Split(new[] { ':' }, 2);
            var length = int.Parse(parts[0]);
            var value = parts[1];

            var bstring = BencodeReader.Read<BString>(encoding, bencode);

            Assert.Equal(length, bstring.Length);
            Assert.Equal(value, bstring.Value);
        }

        [Fact]
        public void CanParse_EmptyString()
        {
            var bstring = BencodeReader.Read<BString>(encoding, "0:");

            Assert.Equal(0, bstring.Length);
            Assert.Equal(string.Empty, bstring.Value);
        }

        [Theory]
        [InlineData("5:spam")]
        [InlineData("6:spam")]
        [InlineData("100:spam")]
        public void LessCharsThanSpecified_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<ParsingException>();
        }

        [Theory]
        [InlineData("4spam")]
        [InlineData("10spam")]
        [InlineData("4-spam")]
        [InlineData("4.spam")]
        [InlineData("4;spam")]
        [InlineData("4,spam")]
        [InlineData("4|spam")]
        public void MissingDelimiter_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("spam")]
        [InlineData("-spam")]
        [InlineData(".spam")]
        [InlineData(",spam")]
        [InlineData(";spam")]
        [InlineData("?spam")]
        [InlineData("!spam")]
        [InlineData("#spam")]
        public void NonDigitFirstChar_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("0")]
        [InlineData("4")]
        public void LessThanMinimumLength2_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("12345678901:spam")]
        [InlineData("123456789012:spam")]
        [InlineData("1234567890123:spam")]
        [InlineData("12345678901234:spam")]
        public void LengthAboveMaxDigits10_ThrowsUnsupportedException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("12:spam")]
        [InlineData("123:spam")]
        [InlineData("1234:spam")]
        [InlineData("12345:spam")]
        [InlineData("123456:spam")]
        [InlineData("1234567:spam")]
        [InlineData("12345678:spam")]
        [InlineData("123456789:spam")]
        [InlineData("1234567890:spam")]
        public void LengthAtOrBelowMaxDigits10_DoesNotThrowUnsupportedException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void LengthAboveInt32MaxValue_ThrowsUnsupportedException()
        {
            var bencode = "2147483648:spam";

            Action action = () => BencodeReader.Read<BString>(encoding, bencode);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void LengthBelowInt32MaxValue_DoesNotThrowUnsupportedException()
        {
            var bencode = "2147483647:spam";

            Action action = () => BencodeReader.Read<BString>(encoding, bencode);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void CanParseEncodedAsLatin1()
        {
            var encoding = Encoding.GetEncoding("LATIN1");
            string value = "3:זרו";

            var bstring = BencodeReader.Read<BString>(encoding, value);

            Assert.Equal(3, bstring.Length);
            Assert.Equal("זרו", bstring.Value);
        }

        [Theory]
        [InlineData("1-:a")]
        [InlineData("1abc:a")]
        [InlineData("123?:asdf")]
        [InlineData("3abc:abc")]
        public void InvalidLengthString_ThrowsInvalidException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        public void BelowMinimumLength_WhenStreamWithoutLengthSupport_ThrowsInvalidException(string bencode)
        {
            Action action = () => BencodeReader.Read<BString>(encoding, bencode);
            action.Should().Throw<Exception>();
        }
    }
}