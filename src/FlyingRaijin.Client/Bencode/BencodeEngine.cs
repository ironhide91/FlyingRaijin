using FlyingRaijin.Client.Torrent;
using System;
using System.Text;

namespace FlyingRaijin.Client.Bencode
{
    public sealed class BencodeEngine : IBencodeEngine
    {
        public BencodeEngine()
        {

        }

        public ITorrent Read(Encoding encoding, string bencodeValue)
        {
            throw new NotImplementedException();
        }

        public string Write(Encoding encoding, string path)
        {
            throw new NotImplementedException();
        }
    }
}