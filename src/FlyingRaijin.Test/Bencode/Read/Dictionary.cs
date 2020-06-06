﻿using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.BObject;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class Dictionary
    {
        [Theory]
        [InlineData("de")]
        public void CanParseEmpty(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BDictionary>();
            result.BObject.Value.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("d4:spam3:egge")]
        public void Case1(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BDictionary>();
            result.BObject.Value.Count.Should().Be(1);

            var dict = result.BObject;
            dict.ContainsKey("spam").Should().BeTrue();

            var value = (BString)dict["spam"];
            value.Value.Length.Should().Be(3);
            value.ToString().Should().Be("egg");
        }


        [Theory]
        [InlineData("d4:spam3:egg3:cow3:mooe")]
        public void Case2(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BDictionary>();
            result.BObject.Value.Count.Should().Be(2);

            var dict = result.BObject;

            dict.ContainsKey("spam").Should().BeTrue();
            var value1 = (BString)dict["spam"];
            value1.Value.Length.Should().Be(3);
            value1.ToString().Should().Be("egg");

            dict.ContainsKey("cow").Should().BeTrue();
            var value2 = (BString)dict["cow"];
            value2.Value.Length.Should().Be(3);
            value2.ToString().Should().Be("moo");
        }

        [Theory]
        [InlineData("d4:spam3:egg3:cow3:moo3:inti99e6:numberi753ee")]
        public void Case3(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BDictionary>();
            result.BObject.Value.Count.Should().Be(4);

            var dict = result.BObject;

            dict.ContainsKey("spam").Should().BeTrue();
            var value1 = (BString)dict["spam"];
            value1.Value.Length.Should().Be(3);
            value1.ToString().Should().Be("egg");

            dict.ContainsKey("cow").Should().BeTrue();
            var value2 = (BString)dict["cow"];
            value2.Value.Length.Should().Be(3);
            value2.ToString().Should().Be("moo");

            dict.ContainsKey("int").Should().BeTrue();
            var value3 = (BInteger)dict["int"];
            value3.Value.Should().Be(99L);

            dict.ContainsKey("number").Should().BeTrue();
            var value4 = (BInteger)dict["number"];
            value4.Value.Should().Be(753L);
        }

        [Theory]
        [InlineData("d4:spam3:egg3:cow3:moo3:inti99e6:numberi753e4:listl5:rahul5:bipini123456789eee")]
        public void Case4(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BDictionary>();
            result.BObject.Value.Count.Should().Be(5);

            var dict = result.BObject;

            dict.ContainsKey("spam").Should().BeTrue();
            var value1 = (BString)dict["spam"];
            value1.Value.Length.Should().Be(3);
            value1.ToString().Should().Be("egg");

            dict.ContainsKey("cow").Should().BeTrue();
            var value2 = (BString)dict["cow"];
            value2.Value.Length.Should().Be(3);
            value2.ToString().Should().Be("moo");

            dict.ContainsKey("int").Should().BeTrue();
            var value3 = (BInteger)dict["int"];
            value3.Value.Should().Be(99L);

            dict.ContainsKey("number").Should().BeTrue();
            var value4 = (BInteger)dict["number"];
            value4.Value.Should().Be(753L);

            dict.ContainsKey("list").Should().BeTrue();
            var value5 = (BList)dict["list"];
            value5.Value.Count.Should().Be(3);

            var value50 = (BString)value5.Value.ElementAt(0);
            value50.Value.Length.Should().Be(5);
            value50.ToString().Should().Be("rahul");

            var value51 = (BString)value5.Value.ElementAt(1);
            value51.Value.Length.Should().Be(5);
            value51.ToString().Should().Be("bipin");

            var value52 = (BInteger)value5.Value.ElementAt(2);
            value52.Value.Should().Be(123456789L);
        }
    }
}