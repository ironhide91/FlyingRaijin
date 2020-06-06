using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class String
    {
        [Theory]
        [InlineData("1:s")]
        [InlineData("4:spam")]
        [InlineData("8:spameggs")]
        [InlineData("9:spam eggs")]
        [InlineData("9:spam:eggs")]
        [InlineData("12:!@#%&/()=?$|")]
        public void CanParseSimple(string bencode)
        {
            var parts = bencode.Split(new[] { ':' }, 2);
            var length = int.Parse(parts[0]);
            var value = parts[1];

            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().NotBeNull();
            result.BObject.Should().BeOfType<BString>();
            result.BObject.Value.Should().NotBeNull();
            result.BObject.Value.Length.Should().Be(length);
            result.BObject.ToString().Should().Be(value);
        }

        [Fact]
        public void CanParseEmptyString()
        {
            var result = BencodeParser.Parse<BString>("0:".AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().NotBeNull();
            result.BObject.Should().BeOfType<BString>();
            result.BObject.Value.Should().NotBeNull();
            result.BObject.Value.Length.Should().Be(0);
            result.BObject.ToString().Should().Be(string.Empty);
        }

        [Theory]
        [InlineData("5:spam")]
        [InlineData("6:spam")]
        [InlineData("100:spam")]
        public void InvalidLessCharsThanSpecified(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringLessCharsThanSpecified);
            result.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("4spam")]
        [InlineData("10spam")]
        [InlineData("4-spam")]
        [InlineData("4.spam")]
        [InlineData("4;spam")]
        [InlineData("4,spam")]
        [InlineData("4|spam")]
        public void InvalidMissingDelimiter(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringInvalid);
            result.BObject.Should().BeNull();
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
        public void InvalidNonDigitFirstChar(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.Unknown);
            result.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("0")]
        [InlineData("4")]
        public void InvalidLessThanMinimumLength(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringInvalid);
            result.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("12345678901:spam")]
        [InlineData("123456789012:spam")]
        [InlineData("1234567890123:spam")]
        [InlineData("12345678901234:spam")]
        public void InvalidLengthAboveMaxDigits10(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringInvalidStringLength);
            result.BObject.Should().BeNull();
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
        public void InvalidLengthAtOrBelowMaxDigits10(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringLessCharsThanSpecified);
            result.BObject.Should().BeNull();
        }

        [Fact]
        public void InvalidLengthAboveInt32MaxValue()
        {
            var bencode = "2147483648:spam";

            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringInvalidStringLength);
            result.BObject.Should().BeNull();
        }

        [Fact]
        public void InvalidLengthNearInt32MaxValue()
        {
            var bencode = "2147483647:spam";

            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringInvalidStringLength);
            result.BObject.Should().BeNull();
        }

        [Fact]
        public void CanParseUnicode()
        {
            var unicodeStr = "$€£¥¢₹₨₱₩฿₫₪©®℗™℠αβγδεζηθικλμνξοπρστυφχψωΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";
            var length = Encoding.UTF8.GetBytes(unicodeStr).Length;
            var utf8Str = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes($"{length}:{unicodeStr}"));

            var result = BencodeParser.Parse<BString>(utf8Str.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Value.Length.Should().Be(length);
            result.BObject.ToString().Should().Be(unicodeStr);
        }

        [Theory]
        [InlineData("1-:a")]
        [InlineData("1abc:a")]
        [InlineData("123?:asdf")]
        [InlineData("3abc:abc")]
        public void InvalidLengthString(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.StringInvalid);
            result.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        public void InvalidBelowMinimumLength(string bencode)
        {
            var result = BencodeParser.Parse<BString>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().NotBe(ErrorType.None);
            result.BObject.Should().BeNull();
        }
    }
}