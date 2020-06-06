using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System.Linq;
using Xunit;

namespace FlyingRaijin.Test.Engine.BencodeStringLiterals
{
    public class SharedMetaData
    {
        [Theory]
        [InlineData("d4:infodee")]
        public void CanReadInfoKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadInfo();

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData("d3:nfo6:samplee")]
        public void MissingInfoKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadInfo();

            temp.Should().BeNull();
        }

        [Theory]
        [InlineData("d8:announce10:sample.come")]
        public void CanReadAnnounceUrlKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadAnnounceUrl();

            temp.Should().NotBeNull();
            temp.Should().Be("sample.com");
        }

        [Theory]
        [InlineData("d7:nnounce10:sample.come")]
        public void MissingAnnounceUrlKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var info = result.BObject.ReadAnnounceUrl();

            info.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d13:announce-listll9:tracker119:tracker129:tracker13eee")]
        public void CanReadAnnounceListKeySingle(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadAnnounceList();

            temp.Should().NotBeNull();
            temp.Tiers.Should().ContainSingle();
            temp.Tiers.First().Should().HaveCount(3);

            temp.Tiers.First()[0].Should().Be("tracker11");
            temp.Tiers.First()[1].Should().Be("tracker12");
            temp.Tiers.First()[2].Should().Be("tracker13");
        }

        [Theory]
        [InlineData("d13:announce-listll9:tracker119:tracker129:tracker13el9:tracker219:tracker22eee")]
        public void CanReadAnnounceListKeyMulti(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadAnnounceList();

            temp.Should().NotBeNull();
            temp.Tiers.Should().HaveCount(2);

            temp.Tiers[0].Should().HaveCount(3);
            temp.Tiers[0][0].Should().Be("tracker11");
            temp.Tiers[0][1].Should().Be("tracker12");
            temp.Tiers[0][2].Should().Be("tracker13");

            temp.Tiers[1][0].Should().Be("tracker21");
            temp.Tiers[1][1].Should().Be("tracker22");
        }

        [Theory]
        [InlineData("d12:announcelistll9:tracker119:tracker129:tracker13eee")]
        public void MissingAnnounceListKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadAnnounceList();

            temp.Should().NotBeNull();
            temp.Tiers.Should().NotBeNull();
            temp.Tiers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d13:announce-listlll7:trackere9:tracker129:tracker13el9:tracker219:tracker22eee")]
        public void ThreeLeveNestedAnnounceList(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadAnnounceList();

            temp.Should().NotBeNull();
            temp.Tiers.Should().NotBeNull();
            temp.Tiers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d13:creation datei669051030ee")]
        public void CanReadCreationDateKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadCreationDate();

            temp.Should().Be(669051030L);
        }

        [Theory]
        [InlineData("d12:creationdatei669051030ee")]
        public void MissingCreationDateKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadCreationDate();

            temp.Should().Be(0L);
        }

        [Theory]
        [InlineData("d7:comment5:helloe")]
        public void CanReadCommentKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadComment();

            temp.Should().NotBeNull();
            temp.Should().Be("hello");
        }

        [Theory]
        [InlineData("d6:omment5:helloe")]
        public void MissingCommentKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadComment();

            temp.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d10:created by4:alexe")]
        public void CanReadCreatedByKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadCreatedBy();

            temp.Should().NotBeNull();
            temp.Should().Be("alex");
        }

        [Theory]
        [InlineData("d9:reated by4:alexe")]
        public void MissingCreatedByKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadCreatedBy();

            temp.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d8:encoding5:UTF-8e")]
        public void CanReadEncodingKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadEncoding();

            temp.Should().NotBeNull();
            temp.Should().Be("UTF-8");
        }

        [Theory]
        [InlineData("d7:ncoding5:UTF-8e")]
        public void MissingEncodingKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadEncoding();

            temp.Should().BeEmpty();
        }
    }
}