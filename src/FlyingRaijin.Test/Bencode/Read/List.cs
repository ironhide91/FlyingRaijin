using FluentAssertions;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using System;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class List
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        [Theory]
        [InlineData("l4:spame")]
        public void CanParseSimple1(string bencode)
        {
            var bList = BencodeReader.Read<BList>(encoding, bencode);

            Assert.Single(bList.Value);
            bList.Value[0].Should().BeOfType<BString>();

            var stringElement = (BString)bList.Value[0];
            Assert.Equal(4, stringElement.Length);
            Assert.Equal("spam", stringElement.Value);
        }

        [Theory]
        [InlineData("l4:spami42ee")]
        public void CanParseSimple2(string bencode)
        {
            var bList = BencodeReader.Read<BList>(encoding, bencode);

            Assert.Equal(2, bList.Value.Count);

            // element 1
            bList.Value[0].Should().BeOfType<BString>(); 
            var stringElement = (BString)bList.Value[0];
            Assert.Equal(4, stringElement.Length);
            Assert.Equal("spam", stringElement.Value);

            // element 2
            bList.Value[1].Should().BeOfType<BInteger>(); 
            var integerElement = (BInteger)bList.Value[1];
            Assert.Equal(42, integerElement.Value);
        }

        [Theory]
        [InlineData("l5:Hello6:World!li123ei456eeetesting")]
        public void CanParseNested1(string bencode)
        {
            var bList = BencodeReader.Read<BList>(encoding, bencode);

            Assert.Equal(3, bList.Value.Count);

            // element 1
            bList.Value[0].Should().BeOfType<BString>();
            var element1 = (BString)bList.Value[0];
            Assert.Equal(5, element1.Length);
            Assert.Equal("Hello", element1.Value);

            // element 2
            bList.Value[1].Should().BeOfType<BString>();
            var element2 = (BString)bList.Value[1];
            Assert.Equal(6, element2.Length);
            Assert.Equal("World!", element2.Value);

            // element 3
            bList.Value[2].Should().BeOfType<BList>();
            var element3 = (BList)bList.Value[2];
            Assert.Equal(2, element3.Value.Count);

            // element 3.0
            element3.Value[0].Should().BeOfType<BInteger>();
            var element31 = (BInteger)element3.Value[0];
            Assert.Equal(123, element31.Value);

            // element 3.1
            element3.Value[1].Should().BeOfType<BInteger>();
            var element32 = (BInteger)element3.Value[1];
            Assert.Equal(456, element32.Value);
        }

        [Theory]
        [InlineData("le")]
        public void CanParseEmptyList(string bencode)
        {
            var bList = BencodeReader.Read<BList>(encoding, bencode);

            Assert.Empty(bList.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("l")]
        public void BelowMinimumLength2_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BList>(encoding, bencode);

            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("4")]
        [InlineData("a")]
        [InlineData(":")]
        [InlineData("-")]
        [InlineData(".")]
        [InlineData("e")]
        public void BelowMinimumLength2_WhenStreamLengthNotSupported_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BList>(encoding, bencode);

            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("ae")]
        [InlineData("ee")]
        [InlineData(":e")]
        [InlineData("4e")]
        [InlineData("-e")]
        [InlineData(".e")]
        public void InvalidFirstChar_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BList>(encoding, bencode);

            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("l4:spam")]
        [InlineData("l ")]
        [InlineData("l:")]
        public void MissingEndChar_ThrowsInvalidBencodeException(string bencode)
        {
            Action action = () => BencodeReader.Read<BList>(encoding, bencode);

            action.Should().Throw<Exception>();
        }
    }
}