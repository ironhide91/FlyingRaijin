using FlyingRaijin.Bencode.Read.ClrObject;
using SimpleBase;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlyingRaijin.Engine.Torrent
{
    public static partial class MetaDataHelper
    {
        private const string InfoPieceLengthKey = "piece length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadPieceLength(this BDictionary bDict)
            => bDict.GetValue<BInteger>(InfoPieceLengthKey).Value;

        private const string InfoPrivateKey = "private";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadIsPrivateFlag(this BDictionary bDict)
            => bDict.GetValue<BInteger>(InfoPrivateKey).Value == 1;

        private const string InfoPiecesKey = "pieces";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pieces ReadPieces(this BDictionary bDict)
        {
            var str = bDict.GetValue<BString>(InfoPiecesKey).Value;

            if (string.IsNullOrEmpty(str))
                return Pieces.Empty;

            byte[] bytes = Encoding.Default.GetBytes(str);
            var utf8Str = Encoding.UTF8.GetString(bytes).AsSpan().Slice(2);

            var sha1Pieces = Base16.Decode(utf8Str);

            var isNotMultipleOf20 = (sha1Pieces.Length % 20) != 0;

            if (isNotMultipleOf20)
                return Pieces.Empty;

            int start = 0, end = (sha1Pieces.Length - 21);

            var sha1Checksumns = new List<byte[]>();

            while (start <= end)
            {
                var sha1 = sha1Pieces.Slice(start, 20);

                sha1Checksumns.Add(sha1.ToArray());

                start += 20;
            }

            return new Pieces(ImmutableList.CreateRange(sha1Checksumns));
        }
    }
}