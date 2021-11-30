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

            var torrent = BencodeEngine.ParseAndReadMetaData(File.ReadAllBytes(filePath).AsSpan());

            // Root
            torrent.Should().NotBeNull();
            torrent.AnnounceUrl.Should().Be("http://linuxtracker.org:2710/00000000000000000000000000000000/announce");
            torrent.CreatedBy.Should().Be(string.Empty);
            torrent.CreationDate.Should().Be(DateTime.Parse("06-11-2013 01:32:47 AM"));
            torrent.Encoding.Should().Be(string.Empty);
            torrent.Comment.Should().Be(string.Empty);

            // Info
            torrent.Should().NotBeNull();
            torrent.Name.Should().Be("slackware64-14.1-iso");
            torrent.PieceLength.Should().Be(2097152);
            torrent.IsPrivate.Should().Be(false);

            // Files
            torrent.Files.Should().NotBeNull();
            torrent.Files.Collection.Should().NotBeEmpty();
            torrent.Files.Collection.Count.Should().Be(4);

            // File 1
            torrent.Files[0].Path.Should().NotBeNullOrEmpty();
            torrent.Files[0].Path.Should().Be("slackware64-14.1-install-dvd.iso");
            torrent.Files[0].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Files[0].LengthInBytes.Should().Be(2438987776);

            // File 2
            torrent.Files[1].Path.Should().NotBeNullOrEmpty();
            torrent.Files[1].Path.Should().Be("slackware64-14.1-install-dvd.iso.asc");
            torrent.Files[1].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Files[1].LengthInBytes.Should().Be(198);

            // File 3
            torrent.Files[2].Path.Should().NotBeNullOrEmpty();
            torrent.Files[2].Path.Should().Be("slackware64-14.1-install-dvd.iso.md5");
            torrent.Files[2].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Files[2].LengthInBytes.Should().Be(67);

            // File 4
            torrent.Files[3].Path.Should().NotBeNullOrEmpty();
            torrent.Files[3].Path.Should().Be("slackware64-14.1-install-dvd.iso.txt");
            torrent.Files[3].Md5Checksum.Should().BeNullOrEmpty();
            torrent.Files[3].LengthInBytes.Should().Be(231925);
        }
    }
}