using FluentAssertions;
using FlyingRaijin.Engine.Bencode;
using System;
using System.IO;
using Xunit;

namespace FlyingRaijin.Test.Engine.TorrentFiles
{
    public class SingleFileTorrent
    {
        [Theory]
        [InlineData("Artifacts\\Torrents\\linuxmint-18-cinnamon-64bit.torrent")]
        public void CanReadInfoKey(string filePath)
        {
            var torrent = BencodeEngine.Instance.ReadsingleFile(File.ReadAllBytes(filePath).AsSpan());

            torrent.Should().NotBeNull();
        }
    }
}