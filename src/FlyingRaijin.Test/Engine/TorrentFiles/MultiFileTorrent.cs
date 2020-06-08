using FluentAssertions;
using FlyingRaijin.Engine.Bencode;
using System;
using System.IO;
using Xunit;

namespace FlyingRaijin.Test.Engine.TorrentFiles
{
    public class MultiFileTorrent
    {
        [Fact]
        public void Multifile()
        {
            var filePath = "Artifacts\\Torrents\\Slackware64-14.1-install-dvd.torrent";

            var torrent = BencodeEngine.Instance.ReadMultiFile(File.ReadAllBytes(filePath).AsSpan());

            // Root
            torrent.Should().NotBeNull();

            torrent.AnnounceUrl.Should().Be("http://linuxtracker.org:2710/00000000000000000000000000000000/announce");

            torrent.CreatedBy.Should().Be(string.Empty);

            torrent.CreationDate.Should().Be(DateTime.Parse("06-11-2013 01:32:47 AM"));

            torrent.Encoding.Should().Be(string.Empty);

            torrent.Comment.Should().Be(string.Empty);

            // Info
            torrent.Info.Should().NotBeNull();
            torrent.Info.DirectoryName.Should().Be("slackware64-14.1-iso");
            torrent.Info.PieceLength.Should().Be(2097152);
            torrent.Info.IsPrivate.Should().Be(false);

            // Files
            torrent.Info.Files.Should().NotBeNull();
            torrent.Info.Files.Should().NotBeEmpty();
            torrent.Info.Files.Count.Should().Be(4);

            // File 1
            torrent.Info.Files[0].Path.Should().NotBeNullOrEmpty();
            torrent.Info.Files[0].Path.Should().Be("slackware64-14.1-install-dvd.iso");
            torrent.Info.Files[0].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Info.Files[0].LengthInBytes.Should().Be(2438987776);

            // File 2
            torrent.Info.Files[1].Path.Should().NotBeNullOrEmpty();
            torrent.Info.Files[1].Path.Should().Be("slackware64-14.1-install-dvd.iso.asc");
            torrent.Info.Files[1].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Info.Files[1].LengthInBytes.Should().Be(198);

            // File 3
            torrent.Info.Files[2].Path.Should().NotBeNullOrEmpty();
            torrent.Info.Files[2].Path.Should().Be("slackware64-14.1-install-dvd.iso.md5");
            torrent.Info.Files[2].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Info.Files[2].LengthInBytes.Should().Be(67);

            // File 4
            torrent.Info.Files[3].Path.Should().NotBeNullOrEmpty();
            torrent.Info.Files[3].Path.Should().Be("slackware64-14.1-install-dvd.iso.txt");
            torrent.Info.Files[3].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Info.Files[3].LengthInBytes.Should().Be(231925);
        }
    }
}