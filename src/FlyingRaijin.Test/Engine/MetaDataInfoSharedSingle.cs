using FluentAssertions;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Engine.Torrent;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Engine
{
    public class MetaDataInfoSharedSingle
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        [Theory]
        [InlineData("d4:name10:ironhide91e")]
        public void CanReadNameInfoSingleKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadSingleName();

            result.Should().NotBeNull();
            result.Should().Be("ironhide91");
        }

        [Theory]
        [InlineData("d3:nam10:ironhide91e")]
        public void MissingNameInfoSingleKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadSingleName();

            result.Should().BeNull();
        }

        [Theory]
        [InlineData("d6:lengthi512ee")]
        public void CanReadLengthInfoSingleKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadSingleLength();

            result.Should().Be(512L);
        }

        [Theory]
        [InlineData("d5:engthi512ee")]
        public void MissingLengthInfoSingleKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadSingleLength();

            result.Should().Be(0L);
        }

        [Theory]
        [InlineData("d6:md5sum32:79054025255fb1a26e4bc422aef54eb4e")]
        public void CanReadMd5ChecksumInfoSingleKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadSingleMD5Checksum();

            result.Should().NotBeNull();
            result.Should().Be("79054025255fb1a26e4bc422aef54eb4");
        }

        [Theory]
        [InlineData("d5:d5sum32:79054025255fb1a26e4bc422aef54eb4e")]
        public void MissingMd5ChecksumInfoSingleKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadSingleMD5Checksum();

            result.Should().BeNull();
        }
    }
}