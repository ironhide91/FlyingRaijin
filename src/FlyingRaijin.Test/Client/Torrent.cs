using System.IO;
using Xunit;

namespace FlyingRaijin.Bencode.Client
{
    public class Torrent
    {
        [Fact]
        public void Test()
        {
            var hex = File.ReadAllText(@"Client\hex.txt");
            //var bytes = hex.HexToByteArray();
        }
    }
}