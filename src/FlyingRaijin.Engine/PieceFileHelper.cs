using System;
using System.Runtime.CompilerServices;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Wire;

namespace FlyingRaijin.Engine
{
    internal static class PieceFileHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValueTuple<long, int> DetermineSlice(CompletePiece piece)
        {
            var (overlaps, file) = PieceOverlaps(piece);

            if (overlaps)
            {
                long pieceEnd = piece.PieceIndex + piece.MetaData.PieceLength - 1;

                if ((file.PieceIndexBegin == piece.PieceIndex) && (file.PieceIndexEnd >= pieceEnd))
                {
                    var begin = file.PieceIndexBegin;
                    var length = file.PieceIndexBegin + piece.MetaData.PieceLength - 1;

                    return (begin, (int)length);
                }

                if (file.PieceIndexBegin < piece.PieceIndex)
                {

                }
            }

            return (0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ValueTuple<bool, FileUnit> PieceOverlaps(CompletePiece piece)
        {
            int pieceEnd = (int)(piece.PieceIndex + piece.MetaData.PieceLength - 1);

            foreach (var file in piece.MetaData.Files.Collection)
            {
                if (file.PieceIndexBegin < piece.PieceIndex)
                {
                    continue;
                }

                if ((file.PieceIndexBegin <= piece.PieceIndex) && (file.PieceIndexEnd >= pieceEnd))
                {
                    return (false, file);
                }

                return (true, file);
            }

            return (true, default);
        }
    }
}