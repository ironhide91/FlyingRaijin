using FluentAssertions;
using FlyingRaijin.Engine.Bencode;
using System.IO;
using System.Text;
using Xunit;

namespace FlyingRaijin.Test.Engine.TorrentFiles
{
    public class SingleFileTorrent
    {
        [Theory]
        [InlineData("Artifacts\\Torrents\\linuxmint-18-cinnamon-64bit.torrent")]
        public void CanReadInfoKey(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var torrent = BencodeEngine.Instance.ReadsingleFile(stream);

                torrent.Should().NotBeNull();
            }
        }
    }
}