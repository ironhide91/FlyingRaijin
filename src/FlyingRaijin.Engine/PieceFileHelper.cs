using System;
using System.Runtime.CompilerServices;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Wire;

namespace FlyingRaijin.Engine
{
    internal static class PieceFileHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValueTuple<FileUnit, long, int, int> DetermineSlice(CompletePiece piece)
        {
            var (overlaps, file) = PieceOverlaps(piece);

            long offset = long.MinValue;
            int begin = int.MinValue;
            int length = int.MinValue;

            if (overlaps)
            {
                long pieceEnd = piece.PieceIndex + piece.MetaData.PieceLength - 1;                

                // we have 5 possibilities now

                // --|-----p----|-----------
                // ------|-------f------|---
                if ((piece.PieceIndex < file.PieceIndexBegin) && (pieceEnd < file.PieceIndexEnd))
                {
                    begin = (int)(file.PieceIndexBegin - piece.PieceIndex);
                    length = (int)(file.PieceIndexBegin + pieceEnd);

                    return (file, 0L, begin, length);
                }

                // --|-----p------|---------
                // --|---------f------|-----
                if ((piece.PieceIndex == file.PieceIndexBegin) && (pieceEnd < file.PieceIndexEnd))
                {
                    // our overlap function is incorrect
                    return (file, 0L, 0, 0);
                }

                // ------|-----p----|-------
                // --|---------f--------|---
                if ((piece.PieceIndex > file.PieceIndexBegin) && (pieceEnd < file.PieceIndexEnd))
                {
                    // our overlap function is incorrect
                    return (file, 0L, 0, 0);
                }

                // -----|-----p----|--------
                // --|-----f-------|--------
                if ((piece.PieceIndex > file.PieceIndexBegin) && (pieceEnd == file.PieceIndexEnd))
                {
                    // our overlap function is incorrect
                    return (file, 0L, 0, 0);
                }

                // ------|-----p-------|----
                // --|-----f-------|--------
                if ((piece.PieceIndex > file.PieceIndexBegin) && (pieceEnd > file.PieceIndexEnd))
                {
                    offset = piece.PieceIndex + file.PieceIndexBegin;
                    begin = 0;
                    length = (int)(piece.PieceIndex - file.PieceIndexEnd);

                    return (file, offset, begin, length);
                }
            }

            offset = piece.PieceIndex + file.PieceIndexBegin;
            begin = 0;
            length = (int)(piece.MetaData.PieceLength - 1);

            return (file, offset, begin, length);
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