using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System;
using System.IO;

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

        public SingleFileTorrent ReadsingleFile(ReadOnlySpan<byte> bytes)
        {
            var result = Parser.Parse(bytes);

            var torrent = new SingleFileTorrent((BDictionary)result.BObject);

            return torrent;
        }

        public SingleFileTorrent ReadsingleFile(string filePath)
        {
            var result = Parser.Parse(File.ReadAllBytes(filePath).AsSpan());

            var torrent = new SingleFileTorrent((BDictionary)result.BObject);

            return torrent;
        }

        public MultiFileTorrent ReadMultiFile(ReadOnlySpan<byte> bytes)
        {
            var result = Parser.Parse(bytes);

            var torrent = new MultiFileTorrent((BDictionary)result.BObject);

            return torrent;
        }

        public MultiFileTorrent ReadMultiFile(string filePath)
        {
            var result = Parser.Parse(File.ReadAllBytes(filePath).AsSpan());

            var torrent = new MultiFileTorrent((BDictionary)result.BObject);

            return torrent;
        }
    }
}