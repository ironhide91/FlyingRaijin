using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Torrent;
using System.Text;
using System.Linq;
using Xunit;

namespace FlyingRaijin.Test.Engine.BencodeStringLiterals
{
    public class MetaDataInfoMulti
    {
        [Theory]
        [InlineData("d4:name10:SomeFoldere")]
        public void CanReadNameInfoMultiKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadMultiName();

            temp.Should().NotBeNull();
            temp.Should().Be("SomeFolder");
        }

        [Theory]
        [InlineData("d3:nam10:SomeFoldere")]
        public void MissingNameInfoMultiKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadMultiName();

            temp.Should().BeEmpty();
        }

        [Theory]
        [InlineData("d4:infod5:filesldededeeee")]
        public void CanReadFilesInfoMultiKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadFiles();

            temp.Should().NotBeNull();
            temp.Collection.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("d4:fileldededeee")]
        public void MissingFilesInfoMultiKey(string bencode)
        {
            var result = BencodeParser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            var temp = result.BObject.ReadMultiFiles();

            temp.Should().NotBeNull();
            temp.Collection.Should().BeEmpty();
        }

        [Fact]
        public void CanReadFilesInfoMultiKey1()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append("d");
                strBuilder.Append("4:info");
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
            strBuilder.Append("e");

            var result = BencodeParser.Parse<BDictionary>(strBuilder.ToString().AsReadOnlyByteSpan());
            var temp = result.BObject.ReadFiles();

            temp.Should().NotBeNull();
            temp.Collection.Should().HaveCount(2);

            temp[0].Should().NotBeNull();
            temp[0].LengthInBytes.Should().Be(1024);
            temp[0].Md5Checksum.Should().Be("9e107d9d372bb6826bd81d3542a419d6");
            temp[0].Path.Should().Be("dir1/dir2/file.ext");

            temp[1].Should().NotBeNull();
            temp[1].LengthInBytes.Should().Be(2024);
            temp[1].Md5Checksum.Should().Be("9e107d9d372bb6826bd81d3542a419d6");
            temp[1].Path.Should().Be("dir1/dir2/file.ext");
        }

        [Fact]
        public void Overlap()
        {
            var sb = new StringBuilder();
            sb.Append("d");
                sb.Append("5:files");
                sb.Append("l");
                    sb.Append("d");
                        sb.Append("6:length");
                        sb.Append("i3e");
                        sb.Append("4:path");
                        sb.Append("l5:file1e");
                    sb.Append("e");
                    sb.Append("d");
                        sb.Append("6:length");
                        sb.Append("i7e");
                        sb.Append("4:path");
                        sb.Append("l5:file2e");
                    sb.Append("e");
                    sb.Append("d");
                        sb.Append("6:length");
                        sb.Append("i10e");
                        sb.Append("4:path");
                        sb.Append("l5:file3e");
                    sb.Append("e");
                    sb.Append("d");
                        sb.Append("6:length");
                        sb.Append("i8e");
                        sb.Append("4:path");
                        sb.Append("l5:file4e");
                    sb.Append("e");
                    sb.Append("d");
                        sb.Append("6:length");
                        sb.Append("i2e");
                        sb.Append("4:path");
                        sb.Append("l5:file5e");
                    sb.Append("e");
                sb.Append("e");
            sb.Append("e");

            var parsed = BencodeEngine.Parse(sb.ToString().AsReadOnlyByteSpan());

            var filesMetaData = MetaDataHelper.ReadMultiFiles(parsed.BObject);

            var totalSize = filesMetaData.Collection.Sum(x => x.LengthInBytes);

            totalSize.Should().Be(30);
        }
    }
}