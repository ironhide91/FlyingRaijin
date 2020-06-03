using FluentAssertions;
using FlyingRaijin.Engine.Torrent;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Engine.BencodeStringLiterals
{
    public class MetaDataInfoMulti
    {
        [Theory]
        [InlineData("d4:name10:SomeFoldere")]
        public void CanReadNameInfoMultiKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);
            var result = bDictionary.ReadMultiName();

            result.Should().NotBeNull();
            result.Should().Be("SomeFolder");
        }

        [Theory]
        [InlineData("d3:nam10:SomeFoldere")]
        public void MissingNameInfoMultiKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);
            var result = bDictionary.ReadMultiName();

            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d5:filesldededeee")]
        public void CanReadFilesInfoMultiKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);
            var result = bDictionary.ReadMultiFiles();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("d4:fileldededeee")]
        public void MissingFilesInfoMultiKey(string bencode)
        {
            var bDictionary = BencodeReader.Read<BDictionary>(bencode);
            var result = bDictionary.ReadMultiFiles();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void CanReadFilesInfoMultiKey1()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append("d");
                strBuilder.Append("5:files");
                strBuilder.Append("l");
                    strBuilder.Append("d");
                        strBuilder.Append("6:length");
                        strBuilder.Append("i1024e");
                        strBuilder.Append("6:md5sum");
                        strBuilder.Append("32:9e107d9d372bb6826bd81d3542a419d6");
                        strBuilder.Append("4:path");
                        strBuilder.Append("l");
                            strBuilder.Append("4:dir1");
                            strBuilder.Append("4:dir2");
                            strBuilder.Append("8:file.ext");
                        strBuilder.Append("e");
                    strBuilder.Append("e");
                    strBuilder.Append("d");
                        strBuilder.Append("6:length");
                        strBuilder.Append("i2024e");
                        strBuilder.Append("6:md5sum");
                        strBuilder.Append("32:9e107d9d372bb6826bd81d3542a419d6");
                        strBuilder.Append("4:path");
                        strBuilder.Append("l");
                            strBuilder.Append("4:dir1");
                            strBuilder.Append("4:dir2");
                            strBuilder.Append("8:file.ext");
                        strBuilder.Append("e");
                    strBuilder.Append("e");
                strBuilder.Append("e");
            strBuilder.Append("e");

            var bDictionary = BencodeReader.Read<BDictionary>(strBuilder.ToString());
            var result = bDictionary.ReadMultiFiles();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].Should().NotBeNull();
            result[0].LengthInBytes.Should().Be(1024);
            result[0].Md5Checksum.Should().Be("9e107d9d372bb6826bd81d3542a419d6");
            result[0].Path.Should().Be("dir1/dir2/file.ext");

            result[1].Should().NotBeNull();
            result[1].LengthInBytes.Should().Be(2024);
            result[1].Md5Checksum.Should().Be("9e107d9d372bb6826bd81d3542a419d6");
            result[1].Path.Should().Be("dir1/dir2/file.ext");
        }
    }
}
