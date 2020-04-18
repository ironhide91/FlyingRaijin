using FluentAssertions;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Engine.Torrent;
using System.IO;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Engine
{
    public class CommonInfoDictionary
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        [Theory]
        [InlineData("d12:piece lengthi512ee")]
        public void CanReadPieceLengthInfoKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadPieceLength();

            result.Should().Be(512L);
        }

        [Theory]
        [InlineData("d11:piecelengthi512ee")]
        public void MissingPieceLengthInfoKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadPieceLength();

            result.Should().Be(0L);
        }

        [Theory]
        [InlineData("d7:privatei1ee")]
        public void CanReadPrivateInfoKeyTrue(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadIsPrivateFlag();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("d7:privatei0ee")]
        public void CanReadPrivateInfoKeyFalse(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadIsPrivateFlag();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("d11:rivatei512ee")]
        public void MissingPrivateInfoKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadIsPrivateFlag();

            result.Should().BeFalse();
        }

        [Fact]
        public void CanReadPiecesInfoKey()
        {
            var piecesStr = File.ReadAllText("Artifacts\\Pieces\\PerfectPiecesSample1.txt");

            var strBuilder = new StringBuilder();
            strBuilder.Append("d");
            strBuilder.Append("6:pieces");
            strBuilder.Append(piecesStr.Length);
            strBuilder.Append(":");
            strBuilder.Append(piecesStr);
            strBuilder.Append("e");

            var bDictionary = BencodeReader.Read<BDictionary>(encoding, strBuilder.ToString());
            var result = bDictionary.ReadPieces();

            result.Should().NotBeNull();
            result.Sha1Checksums.Should().NotBeNull();
            result.Sha1Checksums.Should().HaveCount(1218);
        }

        [Theory]
        [InlineData("d6:ieces5:helloe")]
        public void MissingPiecesInfoKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(encoding, bencode);
            var result = bDictionary.ReadPieces();

            result.Should().NotBeNull();
            result.Sha1Checksums.Should().NotBeNull();
            result.Sha1Checksums.Should().HaveCount(0);
        }

        [Fact]
        public void ImperfectPiecesInfoKey()
        {
            var piecesStr = File.ReadAllText("Artifacts\\Pieces\\ImperfectPiecesSample1.txt");

            var strBuilder = new StringBuilder();
            strBuilder.Append("d");
            strBuilder.Append("6:pieces");
            strBuilder.Append(piecesStr.Length);
            strBuilder.Append(":");
            strBuilder.Append(piecesStr);
            strBuilder.Append("e");

            var bDictionary = BencodeReader.Read<BDictionary>(encoding, strBuilder.ToString());
            var result = bDictionary.ReadPieces();

            result.Should().NotBeNull();
            result.Sha1Checksums.Should().NotBeNull();
            result.Sha1Checksums.Should().HaveCount(0);
        }
    }
}