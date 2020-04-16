using FluentAssertions;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Engine.Torrent;
using System.Text;
using System.Linq;
using Xunit;

namespace FlyingRaijin.Bencode.Engine
{
    public class MetaInfo
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        [Theory]
        [InlineData("d4:info6:samplee")]
        public void CanReadInfoKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadInfo();

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData("d3:nfo6:samplee")]
        public void MissingInfoKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadInfo();

            result.Should().NotBeNull();
            result.Value.Should().BeNull();            
        }

        [Theory]
        [InlineData("d8:announce10:sample.come")]
        public void CanReadAnnounceUrlKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadAnnounceUrl();

            result.Should().NotBeNull();
            result.Should().Be("sample.com");
        }

        [Theory]
        [InlineData("d7:nnounce10:sample.come")]
        public void MissingAnnounceUrlKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var info = bDictionary.ReadAnnounceUrl();

            info.Should().BeNull();
        }

        [Theory]
        [InlineData("d13:announce-listll9:tracker119:tracker129:tracker13eee")]
        public void CanReadAnnounceListKeySingle(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadAnnounceList();

            result.Should().NotBeNull();
            result.Tiers.Should().ContainSingle();
            result.Tiers.First().Should().HaveCount(3);

            result.Tiers.First()[0].Should().Be("tracker11");
            result.Tiers.First()[1].Should().Be("tracker12");
            result.Tiers.First()[2].Should().Be("tracker13");
        }

        [Theory]
        [InlineData("d13:announce-listll9:tracker119:tracker129:tracker13el9:tracker219:tracker22eee")]
        public void CanReadAnnounceListKeyMulti(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadAnnounceList();

            result.Should().NotBeNull();
            result.Tiers.Should().HaveCount(2);

            result.Tiers[0].Should().HaveCount(3);
            result.Tiers[0][0].Should().Be("tracker11");
            result.Tiers[0][1].Should().Be("tracker12");
            result.Tiers[0][2].Should().Be("tracker13");

            result.Tiers[1][0].Should().Be("tracker21");
            result.Tiers[1][1].Should().Be("tracker22");
        }

        [Theory]
        [InlineData("d12:announcelistll9:tracker119:tracker129:tracker13eee")]
        public void MissingAnnounceListKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadAnnounceList();

            result.Should().NotBeNull();
            result.Tiers.Should().NotBeNull();
            result.Tiers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d13:announce-listlll7:trackere9:tracker129:tracker13el9:tracker219:tracker22eee")]
        public void ThreeLeveNestedAnnounceList(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadAnnounceList();

            result.Should().NotBeNull();
            result.Tiers.Should().NotBeNull();
            result.Tiers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d13:creation datei669051030ee")]
        public void CanReadCreationDateKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadCreationDate();

            result.Should().Be(669051030L);
        }

        [Theory]
        [InlineData("d12:creationdatei669051030ee")]
        public void MissingCreationDateKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadCreationDate();

            result.Should().Be(0L);
        }

        [Theory]
        [InlineData("d7:comment5:helloe")]
        public void CanReadCommentKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadComment();

            result.Should().NotBeNull();
            result.Should().Be("hello");
        }

        [Theory]
        [InlineData("d6:omment5:helloe")]
        public void MissingCommentKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadComment();

            result.Should().BeNull();
        }

        [Theory]
        [InlineData("d10:created by4:alexe")]
        public void CanReadCreatedByKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadCreatedBy();

            result.Should().NotBeNull();
            result.Should().Be("alex");
        }

        [Theory]
        [InlineData("d9:reated by4:alexe")]
        public void MissingCreatedByKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadCreatedBy();

            result.Should().BeNull();
        }

        [Theory]
        [InlineData("d8:encoding5:UTF-8e")]
        public void CanReadEncodingKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadEncoding();

            result.Should().NotBeNull();
            result.Should().Be("UTF-8");
        }

        [Theory]
        [InlineData("d7:ncoding5:UTF-8e")]
        public void MissingEncodingKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadEncoding();

            result.Should().BeNull();
        }
    }
}