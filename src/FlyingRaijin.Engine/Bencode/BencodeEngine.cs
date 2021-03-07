using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
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

        private static readonly SHA1Managed sHA1Managed = new SHA1Managed();

        public SingleFileTorrent ReadsingleFile(ReadOnlySpan<byte> bytes)
        {
            var result = BencodeParser.Parse(bytes);

            var torrent = new SingleFileTorrent((BDictionary)result.BObject, GenerateInfoHash(result, bytes));

            return torrent;
        }

        public SingleFileTorrent ReadsingleFile(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath).AsSpan();

            var result = BencodeParser.Parse(bytes);

            var torrent = new SingleFileTorrent((BDictionary)result.BObject, GenerateInfoHash(result, bytes));

            return torrent;
        }

        public MultiFileTorrent ReadMultiFile(ReadOnlySpan<byte> bytes)
        {
            var result = BencodeParser.Parse(bytes);

            var torrent = new MultiFileTorrent((BDictionary)result.BObject, GenerateInfoHash(result, bytes));

            return torrent;
        }

        public MultiFileTorrent ReadMultiFile(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath).AsSpan();

            var result = BencodeParser.Parse(bytes);

            var torrent = new MultiFileTorrent((BDictionary)result.BObject, GenerateInfoHash(result, bytes));

            return torrent;
        }

        private static ReadOnlyMemory<byte> GenerateInfoHash(ParseResult result, ReadOnlySpan<byte> bytes)
        {
            if (result.Error != ErrorType.None)
                return null;

            var rawBuffer = ArrayPool<byte>.Shared.Rent(20);

            Span<byte> spanBuffer = rawBuffer;

            var bytesToHash = bytes.Slice(result.InfoBeginIndex, (result.InfoEndIndex - result.InfoBeginIndex+1));

            int read = 0;

            if (sHA1Managed.TryComputeHash(bytesToHash, spanBuffer, out read))
            {
                ReadOnlyMemory<byte> hash = spanBuffer.Slice(0, 20).ToArray().AsMemory();
                ArrayPool<byte>.Shared.Return(rawBuffer);

                var sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash.Span[i].ToString("X2"));
                }

                //var temp = Uri.EscapeUriString(sb.ToString());
                var temp = System.Net.WebUtility.UrlEncode(sb.ToString());

                return hash;
            }

            return null;
        }
    }
}