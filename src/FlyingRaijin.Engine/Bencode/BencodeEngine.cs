using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace FlyingRaijin.Engine.Bencode
{
    internal static class BencodeEngine
    {
#pragma warning disable SYSLIB0021 // Type or member is obsolete
        private static readonly SHA1Managed sha1Managed = new SHA1Managed();
#pragma warning restore SYSLIB0021 // Type or member is obsolete

        internal static ParseResult<BDictionary> Parse(ReadOnlySpan<byte> data)
        {
            return BencodeParser.Parse<BDictionary>(data);
        }

        internal static ParseResult<BDictionary> Parse(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath).AsSpan();

            return BencodeParser.Parse<BDictionary>(bytes);
        }

        internal static MetaData ReadMetaData(ReadOnlySpan<byte> data, ParseResult<BDictionary> result)
        {
            var torrent = new MetaData(result.BObject, GenerateInfoHash(result, data));

            return torrent;
        }

        internal static MetaData ParseAndReadMetaData(ReadOnlySpan<byte> data)
        {
            return ReadMetaData(data, Parse(data));
        }

        private static InfoHash GenerateInfoHash(ParseResult result, ReadOnlySpan<byte> bytes)
        {
            if (result.Error != ErrorType.None)
                return null;

            var buffer = MemoryPool<byte>.Shared.Rent(20);

            var bytesToHash = bytes.Slice(result.InfoBeginIndex, (result.InfoEndIndex - result.InfoBeginIndex + 1));

            if (sha1Managed.TryComputeHash(bytesToHash, buffer.Memory.Span, out _))
            {
                ReadOnlyMemory<byte> hash = buffer.Memory.Slice(0, 20);
                buffer.Dispose();

                var sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash.Span[i].ToString("X2"));
                }

                return new InfoHash(hash);
            }

            buffer.Dispose();
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GenerateHash(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            sha1Managed.TryComputeHash(source, destination, out _);
        }
    }
}