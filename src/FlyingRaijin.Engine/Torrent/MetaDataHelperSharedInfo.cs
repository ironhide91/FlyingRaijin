using FlyingRaijin.Bencode.BObject;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    public static partial class MetaDataHelper
    {
        private const string InfoPieceLengthKey = "piece length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadPieceLength(this BDictionary bDict)
        {
            var result = bDict.GetValue<BInteger>(InfoPieceLengthKey);

            if (result == null)
                return 0L;

            return result.Value;
        }

        private const string InfoPrivateKey = "private";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadIsPrivateFlag(this BDictionary bDict)
        {
            var result = bDict.GetValue<BInteger>(InfoPrivateKey);

            if (result == null)
                return false;

            return (result.Value == 1);
        }

        private const string InfoPiecesKey = "pieces";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pieces ReadPieces(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoPiecesKey);

            if (result == null)
                return Pieces.Empty;

            var isNotMultipleOf20 = (result.Value.Length % 20) != 0;
            if (isNotMultipleOf20)
                return Pieces.Empty;

            //var temp = BitConverter.ToString(result).Replace("-", "");

            int start = 0, end = (result.Value.Length - 20);
            var sha1Checksumns = new List<byte[]>();

            while (start <= end)
            {
                var sha1 = result.Value.Skip(start).Take(20);
                sha1Checksumns.Add(sha1.ToArray());
                start += 20;
            }

            return new Pieces(ImmutableList.CreateRange(sha1Checksumns));
        }
    }
}