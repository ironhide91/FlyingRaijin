using FluentAssertions;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Tracker;
using System;
using System.IO;
using Xunit;

namespace FlyingRaijin.Test.Engine.Announce
{
    public class TrackerRequest
    {
        private const string announceUrl = "https://torrent.ubuntu.com/announce";        

        [Fact]
        public void InfoHash()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithInfoHash(new byte[]
                {
                    209,16,26,43,
                    157,32,40,17,
                    160,94,140,87,
                    197,87,162,11,
                    249,116,220,138
                })
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be($"https://torrent.ubuntu.com/announce?info_hash=%d1%10%1a%2b%9d+(%11%a0%5e%8cW%c5W%a2%0b%f9t%dc%8a&");
        }

        [Fact]
        public void IP()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithIP("255.255.255.255")
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be($"https://torrent.ubuntu.com/announce?ip=255.255.255.255&");
        }

        [Fact]
        public void Port()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithPort(25962)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?port=25962&");
        }

        [Fact]
        public void Uploaded()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithUploaded(362145L)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?uploaded=362145&");
        }

        [Fact]
        public void Downloaded()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithDownloaded(456781L)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?downloaded=456781&");
        }

        [Fact]
        public void Left()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithLeft(753951L)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?left=753951&");
        }

        [Fact]
        public void EventStarted()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithEvent(EventType.Started)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?event=started&");
        }

        [Fact]
        public void EventStopped()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithEvent(EventType.Stopped)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?event=stopped&");
        }

        [Fact]
        public void EventCompleted()
        {
            var url = new TrackerRequestBuilder(announceUrl)
                .WithEvent(EventType.Completed)
                .Build();

            url.Should().NotBeNullOrEmpty();
            url.Should().Be("https://torrent.ubuntu.com/announce?event=completed&");
        }

        [Fact]
        public void CompleteRequest()
        {
            var filePath = "Artifacts\\Torrents\\ubuntu-20.04.2.0-desktop-amd64.iso.torrent";

            var torrent = BencodeEngine.ParseAndReadMetaData(File.ReadAllBytes(filePath).AsSpan());

            var url = new TrackerRequestBuilder(torrent.AnnounceUrl)
                .WithInfoHash(torrent.InfoHash.ToArray())
                .WithPeerId("ABCDEFGHIJKLMNOPQRST")
                .WithIP("255.255.255.255")
                .WithPort(25962)
                .WithUploaded(0)
                .WithDownloaded(0)
                .WithLeft(0)
                .WithEvent(EventType.Started)
                .WithCompact(1)
                .Build();

            var expectedRequest = "https://torrent.ubuntu.com/announce?"
                + "info_hash=K%a4%fb%f7%23%1a%3af%0e%86%89%27%07%d2%5c%13U3%a1j&"
                + "peer_id=ABCDEFGHIJKLMNOPQRST&"
                + "ip=255.255.255.255&"
                + "port=25962&"
                + "uploaded=0&"
                + "downloaded=0&"
                + "left=0&"
                + "event=started&"
                + "compact=1&";

            url.Should().NotBeNullOrEmpty();
            url.Should().Be(expectedRequest);
        }
    }
}