using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Bencode;
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
        public void TrackerRequest1()
        {
            var filePath = "Artifacts\\Torrents\\ubuntu-20.04.1-desktop-amd64.iso.torrent";

            var torrent = BencodeEngine.Instance.ReadsingleFile(File.ReadAllBytes(filePath).AsSpan());

            var temp = HttpUtility.UrlEncode(torrent.InfoHash.ToArray());

            var sb = new StringBuilder();

            //foreach (var item in torrent.InfoHash.ToArray())
            //{
            //    sb.Append(item.ToString("X2"));
            //}

            //var hashString = sb.ToString();           

            //var temp = HttpUtility.UrlEncode();

            sb.Clear();
            sb.Append("https://torrent.ubuntu.com/announce?");
            sb.Append($"info_hash={temp}");
            sb.Append("&peer_id=ABCDEFGHIJKLMNOPQRST");
            sb.Append("&port=25962");
            sb.Append("&uploaded=0");
            sb.Append("&downloaded=0");
            sb.Append("&left=0");
            sb.Append("&event=started");
            sb.Append("&ip=255.255.255.255");

            var response = httpClient.GetStringAsync(sb.ToString()).Result;

            System.Diagnostics.Debug.WriteLine(response);

            var res = Parser.Parse<BDictionary>(response.AsReadOnlyByteSpan());
            
            
        }
    }
}