using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Wire;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine
{
    internal static class PieceFileHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerable<ValueTuple<FileUnit, SliceInfo>> GetWriteInfo(CompletePiece piece)
        {
            var (overlaps, first, last) = PieceOverlaps(piece);

            var list = new List<(FileUnit, SliceInfo)>();

            if (!overlaps)
            {
                var file = piece.MetaData.Files.Collection[first];
                var sliceInfo = DetermineSlice(piece, file);
                list.Add((file, sliceInfo));

                return list;
            }

            for (int i = first; i <= last; i++)
            {
                var file = piece.MetaData.Files.Collection[i];
                var sliceInfo = DetermineSlice(piece, file);
                list.Add((file, sliceInfo));
            }

            return list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ValueTuple<bool, int, int> PieceOverlaps(CompletePiece piece)
        {
            int pieceEnd = (int)(piece.PieceIndex + piece.MetaData.PieceLength - 1);

            var first = int.MinValue;
            var last = int.MinValue;

            var length = piece.MetaData.Files.Collection.Count;
            var files = piece.MetaData.Files.Collection;

            for (int i = 0; i < length; i++)
            {
                if (files[i].PieceIndexBegin < piece.PieceIndex)
                {
                    continue;
                }

                if ((files[i].PieceIndexBegin <= piece.PieceIndex) && (files[i].PieceIndexEnd >= pieceEnd))
                {
                    return (false, i, int.MinValue);
                }

                if (first == int.MinValue)
                {
                    first = i;
                    continue;
                }

                if (files[i].PieceIndexBegin > pieceEnd)
                {
                    break;
                }

                last = first + 1;
            }

            return (true, first, last);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SliceInfo DetermineSlice(CompletePiece piece, FileUnit file)
        {
            long pieceEnd = piece.PieceIndex + piece.MetaData.PieceLength - 1;

            long offset = long.MinValue;
            int begin = int.MinValue;
            int length = int.MinValue;

            // we have 5 possibilities now

            // --|-----p----|-----------
            // ------|-------f------|---
            if ((piece.PieceIndex < file.PieceIndexBegin) && (pieceEnd < file.PieceIndexEnd))
            {
                begin = (int)(file.PieceIndexBegin - piece.PieceIndex);
                length = (int)(file.PieceIndexBegin + pieceEnd);

                return new SliceInfo(0L, begin, length);
            }

            // --|-----p------|---------
            // --|---------f------|-----
            if ((piece.PieceIndex == file.PieceIndexBegin) && (pieceEnd < file.PieceIndexEnd))
            {
                // our overlap function is incorrect
                return new SliceInfo(0L, 0, 0);
            }

            // ------|-----p----|-------
            // --|---------f--------|---
            if ((piece.PieceIndex > file.PieceIndexBegin) && (pieceEnd < file.PieceIndexEnd))
            {
                // our overlap function is incorrect
                return new SliceInfo(0L, 0, 0);
            }

            // -----|-----p----|--------
            // --|-----f-------|--------
            if ((piece.PieceIndex > file.PieceIndexBegin) && (pieceEnd == file.PieceIndexEnd))
            {
                // our overlap function is incorrect
                return new SliceInfo(0L, 0, 0);
            }

            // ------|-----p-------|----
            // --|-----f-------|--------
            if ((piece.PieceIndex > file.PieceIndexBegin) && (pieceEnd > file.PieceIndexEnd))
            {
                offset = piece.PieceIndex + file.PieceIndexBegin;
                begin = 0;
                length = (int)(piece.PieceIndex - file.PieceIndexEnd);

                return new SliceInfo(offset, begin, length);
            }

            return new SliceInfo(long.MinValue, int.MinValue, int.MinValue);
        }        
    }

    internal struct SliceInfo
    {
        internal SliceInfo(long offset, int start, int length)
        {
            Offset = offset;
            Start = start;
            Length = length;
        }

        internal readonly long Offset;
        internal readonly int Start;
        internal readonly int Length;
    }
}