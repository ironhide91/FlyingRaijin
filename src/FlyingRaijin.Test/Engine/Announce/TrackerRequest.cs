using System.Net.Http;
using System.Web;
using Xunit;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using System.Text;
using System;
using System.IO;

namespace FlyingRaijin.Test.Engine.Announce
{
    public class TrackerRequest
    {
        private static readonly HttpClient httpClient = new HttpClient();

        //[Fact]
        public void TrackerRequest1()
        {
            //string str= @"http://legittorrents.info:2710/announce?info_hash=%0A%D8K%FD%EA%91%B2%B43H%F1%BE%0C%9AD%96nr%EC%97&peer_id=b9970db982b16628419f47ebaddc53334a118cc8&port=25962&uploaded=0&downloaded=0&left=314572800&event=started&ip=255.255.255.255&num_want=10";
            //                                           //announce?info_hash=%0A%D8K%FD%EA%91%B2%B43H%F1%BE%0C%9AD%96nr%EC%97&peer_id=TIX0276-f1a2a0i1d6d2&port=25962&uploaded=0&downloaded=0&left=314572800&corrupt=0&key=1Q2O5P0Q&event=started&numwant=100&compact=1&no_peer_id=1 HTTP/1.1\r\n

            //var temp = HttpUtility.UrlEncode("0ad84bfdea91b2b43348f1be0c9a44966e72ec97");

            //var response = httpClient.GetStringAsync(str).Result;

            //int i = 0;
            //foreach (var item in response.AsReadOnlyByteSpan())
            //{
            //    System.Diagnostics.Debug.WriteLine($"{i}-{item}");
            //    i++;
            //}

            //var bencode = "d8:completei6e10:incompletei1e8:intervali1800e12:min intervali1800e5:peers42:g?O?ej?&3???L?× ,X?-Y??S?_^c?#'.??Q??e";
            var bencode = "d8:completei2e10:incompletei1e8:intervali1800e12:min intervali1800e5:peers18:¸:Ñ‘=gÄOìej¸”}¾Ûe";

            //ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(bencode);
            //var bytes = Encoding.UTF8.GetBytes(bencode);

            //char[] bcs = new char[18];
            //Span<char> chars = new Span<char>(bcs);

            ////var str = Encoding.UTF8.GetChars(bytes.Slice(77), chars);
            ////var str = Encoding.UTF8.GetChars(bytes, 77 , , chars);
            //var strc = Encoding.UTF8.GetCharCount(bytes);
            ////strc.is

            //var ms = new MemoryStream(bytes);

            //var streamReader = new StreamReader(ms, Encoding.UTF8);
            //streamReader.BaseStream.Seek(77, SeekOrigin.Begin);
            //var num = streamReader.Read(bcs, 0, 18);

            var res = Parser.Parse<BDictionary>(bencode.AsReadOnlyByteSpan());
            

        }
    }

    //b9970db982b16628419f47ebaddc53334a118cc8
}