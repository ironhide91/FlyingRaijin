using Xunit;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Torrent;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Test
{
    public class Test
    {
        [Fact]
        public void AddFile()
        {
            //var engine = EngineActorSystem.Instance;

            //var filePath = "Artifacts\\Torrents\\Slackware64-14.1-install-dvd.torrent";

            //Controller.Instance.Add(filePath);

            //while (true)
            //{

            //}
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
        }
    }
}