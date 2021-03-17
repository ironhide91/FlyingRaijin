using FlyingRaijin.Bencode.Read;
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

        [Fact]
        public void Case5()
        {
            var bencode = "d8:completei2e10:incompletei1e8:intervali1800e12:min intervali1800e5:peers18:¸:Ñ‘=gÄOìej¸”}¾Ûe";

            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            result.Should().NotBeNull();
            result.Error.Should().Be(ErrorType.None);
            result.BObject.Should().BeOfType<BDictionary>();
            result.BObject.Value.Count.Should().Be(5);

            var dict = result.BObject;

            dict.ContainsKey("complete").Should().BeTrue();
            var value1 = (BInteger)dict["complete"];
            value1.Value.Should().Be(2);

            dict.ContainsKey("incomplete").Should().BeTrue();
            var value2 = (BInteger)dict["incomplete"];
            value2.Value.Should().Be(1);

            dict.ContainsKey("interval").Should().BeTrue();
            var value3 = (BInteger)dict["interval"];
            value3.Value.Should().Be(1800);

            dict.ContainsKey("min interval").Should().BeTrue();
            var value4 = (BInteger)dict["min interval"];
            value4.Value.Should().Be(1800);

            dict.ContainsKey("peers").Should().BeTrue();
            var value5 = (BString)dict["peers"];
            value5.StringValue.Length.Should().Be(18);
            value5.StringValue.Should().Be("¸:Ñ‘=gÄOìej¸”}¾Û");
        }

        [Fact]
        public void Case6()
        {
            var bencode = "d2:ip13:185.149.90.647:peer id20:-lt0D60-�r3�@K�9LU!4:porti51072ee";

            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());

            //result.Should().NotBeNull();
            //result.Error.Should().Be(ErrorType.None);
            //result.BObject.Should().BeOfType<BDictionary>();
            //result.BObject.Value.Count.Should().Be(5);

            //var dict = result.BObject;

            //dict.ContainsKey("complete").Should().BeTrue();
            //var value1 = (BInteger)dict["complete"];
            //value1.Value.Should().Be(2);

            //dict.ContainsKey("incomplete").Should().BeTrue();
            //var value2 = (BInteger)dict["incomplete"];
            //value2.Value.Should().Be(1);

            //dict.ContainsKey("interval").Should().BeTrue();
            //var value3 = (BInteger)dict["interval"];
            //value3.Value.Should().Be(1800);

            //dict.ContainsKey("min interval").Should().BeTrue();
            //var value4 = (BInteger)dict["min interval"];
            //value4.Value.Should().Be(1800);

            //dict.ContainsKey("peers").Should().BeTrue();
            //var value5 = (BString)dict["peers"];
            //value5.StringValue.Length.Should().Be(18);
            //value5.StringValue.Should().Be("¸:Ñ‘=gÄOìej¸”}¾Û");
        }

        [Fact]
        public void Case7()
        {
            var bencode = "d7:peer id20:-lt0D60-�r3�@K�9LU!e";

            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
        }

        [Fact]
        public void Case8()
        {
            var bencode = "d7:peer id20:-lt0D80-�s�u�wSMuԐe";

            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
        }

        //[Fact]
        //public void Case9()
        //{
            

        //    var temp = System.Net.WebUtility.UrlDecode(peerId);

        //    var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
        //}

        //d8:completei3517e10:incompletei56e8:intervali1800e5:peers300:PmK��Cۈ��PC�]�=��LWBW_��ņ/о"��C�"��FR��w�ҹ-þN$[� L�թ,X�8��XO�ˁS�_C����yV�ռ�1{����E��R�����[>|���S���٫�K��}�bP���\�6����j-�N7�T��Zi���1�-�Bh� ��rٿ��֞e`��Y�����%9��HO��"�����W��g�/��������)G��*)�-�؏����pt�Fq�$�h�X����%�dH��ʻ�;'��9��������kGNN�k���RA�$9FH�`]u�e
}
}