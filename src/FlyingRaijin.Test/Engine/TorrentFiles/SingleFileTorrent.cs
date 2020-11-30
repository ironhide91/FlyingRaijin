using FluentAssertions;
using FlyingRaijin.Engine.Bencode;
using System;
using System.IO;
using Xunit;

namespace FlyingRaijin.Test.Engine.TorrentFiles
{
    public class SingleFileTorrent
    {
        [Fact]
        public void SingleFile()
        {
            var filePath = "Artifacts\\Torrents\\linuxmint-18-cinnamon-64bit.torrent";

            var torrent = BencodeEngine.Instance.ReadsingleFile(File.ReadAllBytes(filePath).AsSpan());

            // Root
            torrent.Should().NotBeNull();            
            torrent.AnnounceUrl
                .Should().Be("http://linuxtracker.org:2710/00000000000000000000000000000000/announce");            
            torrent.CreatedBy.Should().Be("Transmission/2.84 (14307)");            
            torrent.CreationDate.Should().Be(DateTime.Parse("2016-06-30 09:35:31"));            
            torrent.Encoding.Should().Be("UTF-8");            
            torrent.Comment.Should().Be(string.Empty);
            // Info
            torrent.Info.Should().NotBeNull();
            torrent.Info.FileName.Should().Be("linuxmint-18-cinnamon-64bit.iso");       
            torrent.Info.FileLengthInBytes.Should().Be(1697906688L);
            torrent.Info.PieceLength.Should().Be(1048576L);            
            torrent.Info.MD5Checksum.Should().Be(string.Empty);
            torrent.Info.IsPrivate.Should().Be(false);
        }
    }
}