using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using Xunit;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class List
    {
        [Theory]
        [InlineData("l4:spame")]
        public void CanParseSimple1(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BList>();
            result.BObject.Value.Count.Should().Be(1);

            result.BObject.Value[0].Should().BeOfType<BString>();
            
            var stringElement = (BString)result.BObject.Value[0];
            stringElement.Value.Length.Should().Be(4);
            stringElement.ToString().Should().Be("spam");
        }

        [Theory]
        [InlineData("l4:spami42ee")]
        public void CanParseSimple2(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BList>();
            result.BObject.Value.Count.Should().Be(2);

            // element 1
            result.BObject.Value[0].Should().BeOfType<BString>();
            var stringElement = (BString)result.BObject.Value[0];
            stringElement.Value.Length.Should().Be(4);
            stringElement.ToString().Should().Be("spam");

            // element 2
            result.BObject.Value[1].Should().BeOfType<BInteger>();
            var integerElement = (BInteger)result.BObject.Value[1];
            integerElement.Value.Should().Be(42);
        }

        [Theory]
        [InlineData("l5:Hello6:World!li123ei456eee")]
        public void CanParseNested1(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BList>();
            result.BObject.Value.Count.Should().Be(3);

            // element 1
            result.BObject.Value[0].Should().BeOfType<BString>();
            var element1 = (BString)result.BObject.Value[0];
            element1.Value.Length.Should().Be(5);
            element1.ToString().Should().Be("Hello");

            // element 2
            result.BObject.Value[1].Should().BeOfType<BString>();
            var element2 = (BString)result.BObject.Value[1];
            element2.Value.Length.Should().Be(6);
            element2.ToString().Should().Be("World!");

            // element 3
            result.BObject.Value[2].Should().BeOfType<BList>();
            var element3 = (BList)result.BObject.Value[2];
            element3.Value.Count.Should().Be(2);

            // element 3.1
            element3.Value[0].Should().BeOfType<BInteger>();
            var element31 = (BInteger)element3.Value[0];
            element31.Value.Should().Be(123);

            // element 3.2
            element3.Value[1].Should().BeOfType<BInteger>();
            var element32 = (BInteger)element3.Value[1];
            element32.Value.Should().Be(456);
        }

        [Theory]
        [InlineData("le")]
        public void CanParseEmptyList(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BList>();
            result.BObject.Value.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData("l")]
        [InlineData("4")]
        [InlineData("a")]
        [InlineData(":")]
        [InlineData("-")]
        [InlineData(".")]
        [InlineData("e")]
        public void InvalidBelowMinimumLength2(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().NotBe(ErrorType.None);
            result.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("ae")]
        [InlineData("ee")]
        [InlineData(":e")]
        [InlineData("4e")]
        [InlineData("-e")]
        [InlineData(".e")]
        public void InvalidFirstChar(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().NotBe(ErrorType.None);
            result.BObject.Should().BeNull();
        }

        [Theory]
        [InlineData("l4:spam")]
        [InlineData("l ")]
        [InlineData("l:")]
        public void InvalidMissingEndChar(string bencode)
        {
            var result = BencodeParser.Parse<BList>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().NotBe(ErrorType.None);
            result.BObject.Should().BeNull();
        }
    }
}