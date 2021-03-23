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
    public static class BencodeEngine
    {
        private static readonly SHA1Managed sha1Managed = new SHA1Managed();

        public static ParseResult<BDictionary> Parse(ReadOnlySpan<byte> data)
        {
            return BencodeParser.Parse<BDictionary>(data);
        }

        public static ParseResult<BDictionary> Parse(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath).AsSpan();

            return BencodeParser.Parse<BDictionary>(bytes);
        }

        public static MetaData ReadMetaData(ReadOnlySpan<byte> data, ParseResult<BDictionary> result)
        {
            var torrent = new MetaData(result.BObject, GenerateInfoHash(result, data));

            return torrent;
        }

        public static MetaData ParseAndReadMetaData(ReadOnlySpan<byte> data)
        {
            return ReadMetaData(data, Parse(data));
        }

        private static ReadOnlyMemory<byte> GenerateInfoHash(ParseResult result, ReadOnlySpan<byte> bytes)
        {
            if (result.Error != ErrorType.None)
                return null;

            var rawBuffer = ArrayPool<byte>.Shared.Rent(20);

            Span<byte> spanBuffer = rawBuffer;

            var bytesToHash = bytes.Slice(result.InfoBeginIndex, (result.InfoEndIndex - result.InfoBeginIndex+1));

            if (sha1Managed.TryComputeHash(bytesToHash, spanBuffer, out _))
            {
                ReadOnlyMemory<byte> hash = spanBuffer.Slice(0, 20).ToArray().AsMemory();

                ArrayPool<byte>.Shared.Return(rawBuffer);

                var sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash.Span[i].ToString("X2"));
                }

                return hash;
            }

            return null;
        }
    }
}