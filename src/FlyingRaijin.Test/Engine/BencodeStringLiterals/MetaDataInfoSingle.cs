using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using Xunit;

namespace FlyingRaijin.Test.Engine.BencodeStringLiterals
{
    public class MetaDataInfoSingle
    {
        [Theory]
        [InlineData("d4:name10:ironhide91e")]
        public void CanReadNameInfoSingleKey(string bencode)
        {
            var result = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadSingleName();

            temp.Should().NotBeNull();
            temp.Should().Be("ironhide91");
        }

        [Theory]
        [InlineData("d3:nam10:ironhide91e")]
        public void MissingNameInfoSingleKey(string bencode)
        {
            var result = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadSingleName();

            temp.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d6:lengthi512ee")]
        public void CanReadLengthInfoSingleKey(string bencode)
        {
            var result = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadSingleLength();

            temp.Should().Be(512L);
        }

        [Theory]
        [InlineData("d5:engthi512ee")]
        public void MissingLengthInfoSingleKey(string bencode)
        {
            var result = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadSingleLength();

            temp.Should().Be(0L);
        }

        [Theory]
        [InlineData("d6:md5sum32:79054025255fb1a26e4bc422aef54eb4e")]
        public void CanReadMd5ChecksumInfoSingleKey(string bencode)
        {
            var result = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadSingleMD5Checksum();

            temp.Should().NotBeNull();
            temp.Should().Be("79054025255fb1a26e4bc422aef54eb4");
        }

        [Theory]
        [InlineData("d5:d5sum32:79054025255fb1a26e4bc422aef54eb4e")]
        public void MissingMd5ChecksumInfoSingleKey(string bencode)
        {
            var result = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadSingleMD5Checksum();

            temp.Should().BeEmpty();
        }
    }
}