using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Engine.Torrent;
using System;
using System.IO;
using System.Text;

namespace FlyingRaijin.Engine.Bencode
{
    public sealed class BencodeEngine : IBencodeEngine
    {
        private BencodeEngine()
        {

        }

        private static readonly Lazy<BencodeEngine> lazy =
            new Lazy<BencodeEngine> (() => new BencodeEngine());

        public static BencodeEngine Instance { get { return lazy.Value; } }

        public SingleFileTorrent ReadsingleFile(Encoding encoding, Stream stream)
        {
            var metaData = BencodeReader.Read<BDictionary>(encoding, stream);

            var torrent = new SingleFileTorrent(metaData);

            return torrent;
        }

        public MultiFileTorrent ReadMultiFile(Encoding encoding, Stream stream)
        {
            var metaData = BencodeReader.Read<BDictionary>(encoding, stream);

            var torrent = new MultiFileTorrent(metaData);

            return torrent;
        }
    }
}