using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Tracker;
using Serilog;
using Serilog.Core;
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

        [Fact]
        public void Serilog()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.File("logs\\myapp.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{SourceContext}")
                .CreateLogger()
                .ForContext(typeof(TrackerRequest));            

            logger.Information("hello");
        }

            [Fact]
        public void TrackerRequest1()
        {
            var filePath = "Artifacts\\Torrents\\ubuntu-20.04.1-desktop-amd64.iso.torrent";

            var torrent = BencodeEngine.Instance.ReadsingleFile(File.ReadAllBytes(filePath).AsSpan());

            var temp = HttpUtility.UrlEncode(torrent.InfoHash.ToArray());

            var sb = new StringBuilder();


            var requestBuilder = new TrackerRequestBuilder(torrent.AnnounceUrl)
                .WithInfoHash(torrent.InfoHash.ToArray())
                .WithIP("255.255.255.255")
                .WithPort(25962)
                .WithUploaded(0)
                .WithDownloaded(0)
                .WithLeft(0)
                .WithEvent(EventType.Started)
                .Build();

            //sb.Clear();
            //sb.Append("https://torrent.ubuntu.com/announce?");
            //sb.Append($"info_hash={temp}");
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

            System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(response));

            //var response = File.ReadAllText("Artifacts\\peers.txt", Encoding.UTF8);

            var res = BencodeParser.Parse<BDictionary>(response);

            var parsedResponse = TrackerResponseParser.Parse(response);




        }
    }
}