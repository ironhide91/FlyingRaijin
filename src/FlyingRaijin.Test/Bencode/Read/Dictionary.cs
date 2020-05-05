using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using System.Linq;
using Xunit;

namespace FlyingRaijin.Test.Bencode.Read
{
    public class Dictionary
    {
        [Theory]
        [InlineData(@"d8:announce70:http://linuxtracker.org:2710/00000000000000000000000000000000/announce13:announce-listll70:http://linuxtracker.org:2710/00000000000000000000000000000000/announceel42:http://torrents.linuxmint.com/announce.phpee10:created by25:Transmission/2.84 (14307)13:creation datei1467279331e8:encoding5:UTF-84:infod6:lengthi1697906688e4:name31:linuxmint-18-cinnamon-64bit.iso12:piece lengthi1048576e6:pieces4:test7:privatei0eee")]
        public void Real(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);
        }

        [Theory]
        [InlineData("de")]
        public void CanParseEmpty(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);

            Assert.Empty(bDictionary.Value);
        }

        [Theory]
        [InlineData("d4:spam3:egge")]
        public void Case1(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);

            Assert.Single(bDictionary.Value);
            Assert.True(bDictionary.Value.ContainsKey("spam"));

            var value = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value.Length);
            Assert.Equal("egg", value.Value);
        }


        [Theory]
        [InlineData("d4:spam3:egg3:cow3:mooe")]
        public void Case2(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);

            Assert.Equal(2, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));
            Assert.True(bDictionary.Value.ContainsKey("cow"));

            var value1 = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value1.Length);
            Assert.Equal("egg", value1.Value);

            var value2 = (BString)bDictionary.Value["cow"];
            Assert.Equal(3, value2.Length);
            Assert.Equal("moo", value2.Value);
        }

        [Theory]
        [InlineData("d4:spam3:egg3:cow3:moo3:inti99e6:numberi753ee")]
        public void Case3(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);

            Assert.Equal(4, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));
            Assert.True(bDictionary.Value.ContainsKey("cow"));
            Assert.True(bDictionary.Value.ContainsKey("int"));
            Assert.True(bDictionary.Value.ContainsKey("number"));

            var value1 = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value1.Length);
            Assert.Equal("egg", value1.Value);

            var value2 = (BString)bDictionary.Value["cow"];
            Assert.Equal(3, value2.Length);
            Assert.Equal("moo", value2.Value);

            var value3 = (BInteger)bDictionary.Value["int"];
            Assert.Equal(99L, value3.Value);

            var value4 = (BInteger)bDictionary.Value["number"];
            Assert.Equal(753L, value4.Value);
        }

        [Theory]
        [InlineData("d4:spam3:egg3:cow3:moo3:inti99e6:numberi753e4:listl5:rahul5:bipini123456789eee")]
        public void Case4(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);

            Assert.Equal(5, bDictionary.Value.Count);
            Assert.True(bDictionary.Value.ContainsKey("spam"));
            Assert.True(bDictionary.Value.ContainsKey("cow"));
            Assert.True(bDictionary.Value.ContainsKey("int"));
            Assert.True(bDictionary.Value.ContainsKey("number"));
            Assert.True(bDictionary.Value.ContainsKey("list"));

            var value1 = (BString)bDictionary.Value["spam"];
            Assert.Equal(3, value1.Length);
            Assert.Equal("egg", value1.Value);

            var value2 = (BString)bDictionary.Value["cow"];
            Assert.Equal(3, value2.Length);
            Assert.Equal("moo", value2.Value);

            var value3 = (BInteger)bDictionary.Value["int"];
            Assert.Equal(99L, value3.Value);

            var value4 = (BInteger)bDictionary.Value["number"];
            Assert.Equal(753L, value4.Value);

            var value5 = (BList)bDictionary.Value["list"];
            Assert.Equal(3, value5.Value.Count);

            var value50 = (BString)value5.Value[0];
            Assert.Equal(5, value50.Length);
            Assert.Equal("rahul", value50.Value);

            var value51 = (BString)value5.Value.ElementAt(1);
            Assert.Equal(5, value51.Length);
            Assert.Equal("bipin", value51.Value);

            var value52 = (BInteger)value5.Value.ElementAt(2);
            Assert.Equal(123456789L, value52.Value);
        }
    }
}
