using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using Xunit;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class Integer
    {
        [Fact]
        public void CanParseZero()
        {
            var bnumber = BencodeParser.Parse<BInteger>("i0e".AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.None);
            bnumber.BObject.Should().BeOfType<BInteger>();
            bnumber.BObject.Value.Should().Be(0);
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
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.None);
            bnumber.BObject.Should().BeOfType<BInteger>();
            bnumber.BObject.Value.Should().Be(value);
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
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.None);
            bnumber.BObject.Should().BeOfType<BInteger>();
            bnumber.BObject.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("i9223372036854775807e", 9223372036854775807)]
        [InlineData("i-9223372036854775808e", -9223372036854775808)]
        public void CanParseInt64(string bencode, long value)
        {
            var temp = long.Parse("-9223372036854775808");


            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.None);
            bnumber.BObject.Should().BeOfType<BInteger>();
            bnumber.BObject.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("i01e")]
        [InlineData("i012e")]
        [InlineData("i01234567890e")]
        [InlineData("i00001e")]
        public void CannotParseLeadingZeros(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerInvalid);
            bnumber.BObject.Should().BeNull();
        }

        [Fact]
        public void CannotParseMinusZero()
        {
            var bnumber = BencodeParser.Parse<BInteger>("i-0e".AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerInvalid);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("i")]
        [InlineData("i1")]
        [InlineData("i2")]
        [InlineData("ie")]
        public void CannotParseLessThanIntegerMinimumLength(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerInvalid);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("i12")]
        [InlineData("i123")]
        [InlineData("i-12")]
        [InlineData("i-123")]
        public void CannotParseMissingEndChar(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerInvalid);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("1e")]
        [InlineData("42e")]
        [InlineData("100e")]
        [InlineData("1234567890e")]
        public void CannotParseInvalidFirstChar(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().NotBe(ErrorType.None);
            bnumber.BObject.Should().BeNull();
        }

        [Fact]
        public void CannotParseOnlyNegativeSign()
        {
            var bnumber = BencodeParser.Parse<BInteger>("i-e".AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerInvalid);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("i--1e")]
        [InlineData("i--42e")]
        [InlineData("i---100e")]
        [InlineData("i----1234567890e")]
        public void CannotParseMultipleNegativeSign(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerInvalid);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("iasdfe")]
        [InlineData("i!#¤%&e")]
        [InlineData("i.e")]
        [InlineData("i42.e")]
        [InlineData("i42ae")]
        public void CannotParseNonInteger(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().NotBe(ErrorType.None);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("i9223372036854775808e")]
        [InlineData("i-9223372036854775809e")]
        public void CannotParseLengthLargerThanInt64(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerOutOfInt64Range);
            bnumber.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("i12345678901234567890e")]
        [InlineData("i123456789012345678901e")]
        [InlineData("i123456789012345678901234567890e")]
        public void CannotParseLongerThanint64MaxDigits19(string bencode)
        {
            var bnumber = BencodeParser.Parse<BInteger>(bencode.AsReadOnlyByteSpan());

            bnumber.Should().NotBeNull();
            bnumber.Error.Should().Be(ErrorType.IntegerOutOfInt64Range);
            bnumber.BObject.Should().BeNull();
        }
    }
}