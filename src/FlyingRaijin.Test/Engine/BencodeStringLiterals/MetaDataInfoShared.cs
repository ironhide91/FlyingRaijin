using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using Xunit;

namespace FlyingRaijin.Test.Engine.BencodeStringLiterals
{
    public class MetaDataInfoShared
    {
        [Theory]
        [InlineData("d4:infod12:piece lengthi512eee")]
        public void CanReadPieceLengthInfoKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadPieceLength();

            temp.Should().Be(512L);
        }

        [Theory]
        [InlineData("d11:piecelengthi512ee")]
        public void MissingPieceLengthInfoKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadPieceLength();

            temp.Should().Be(0L);
        }

        [Theory]
        [InlineData("d7:privatei1ee")]
        public void CanReadPrivateInfoKeyTrue(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadIsPrivateFlag();

            temp.Should().BeTrue();
        }

        [Theory]
        [InlineData("d7:privatei0ee")]
        public void CanReadPrivateInfoKeyFalse(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadIsPrivateFlag();

            temp.Should().BeFalse();
        }

        [Theory]
        [InlineData("d11:rivatei512ee")]
        public void MissingPrivateInfoKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadIsPrivateFlag();

            temp.Should().BeFalse();
        }

        [Theory]
        [InlineData("d6:ieces5:helloe")]
        public void MissingPiecesInfoKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadPieceHash();

            temp.Should().NotBeNull();
            temp.Sha1Checksums.Should().NotBeNull();
            temp.Sha1Checksums.Should().HaveCount(0);
        }
    }
}