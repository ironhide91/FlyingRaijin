using FluentAssertions;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Tracker;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using Xunit;

namespace FlyingRaijin.Test.Engine.Announce
{
    public class TrackerRequest
    {
        private static readonly HttpClient httpClient = new HttpClient();

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

            var torrent = BencodeEngine.Instance.ReadsingleFile(File.ReadAllBytes(filePath).AsSpan());

            var requestBuilder = new TrackerRequestBuilder(torrent.AnnounceUrl)
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

            //var sb = new StringBuilder();
            //sb.Clear();
            //sb.Append("https://torrent.ubuntu.com/announce?");
            //sb.Append($"info_hash={HttpUtility.UrlEncode(torrent.InfoHash.ToArray())}");
            //sb.Append("&peer_id=ABCDEFGHIJKLMNOPQRST");
            //sb.Append("&port=25962");
            //sb.Append("&uploaded=0");
            //sb.Append("&downloaded=0");
            //sb.Append("&left=0");
            //sb.Append("&event=started");
            //sb.Append("&ip=255.255.255.255");
            //sb.Append("&compact=1");
            //sb.Append("no_peer_id=1");

            var response = httpClient.GetByteArrayAsync(requestBuilder).Result;
            //var response = httpClient.GetByteArrayAsync(sb.ToString()).Result;

            System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(response));

            var res = BencodeParser.Parse<BDictionary>(response);

            var parsedResponse = TrackerResponseParser.Parse(response);
        }
    }
}