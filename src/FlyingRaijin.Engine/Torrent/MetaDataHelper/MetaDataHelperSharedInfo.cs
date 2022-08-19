using FlyingRaijin.Bencode.BObject;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    internal static partial class MetaDataHelper
    {
        private const string InfoPieceLengthKey = "piece length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static long ReadPieceLength(this BDictionary bDict)
        {
            var infoDict = bDict.GetValue<BDictionary>(RootInfoKey);

            var result = infoDict.GetValue<BInteger>(InfoPieceLengthKey);

            if (result == null)
                return 0L;

            return result.Value;
        }

        private const string InfoPrivateKey = "private";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool ReadIsPrivateFlag(this BDictionary bDict)
        {
            var result = bDict.GetValue<BInteger>(InfoPrivateKey);

            if (result == null)
                return false;

            return (result.Value == 1);
        }

        private const string InfoPiecesKey = "pieces";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static PieceHash ReadPieceHash(this BDictionary bDict)
        {
            var info = bDict.GetValue<BDictionary>(RootInfoKey);

            if (info == null)
              return PieceHash.Empty;

            var result = info.GetValue<BString>(InfoPiecesKey);

            var isNotMultipleOf20 = (result.Value.Length % 20) != 0;
            if (isNotMultipleOf20)
                return PieceHash.Empty;

            int start = 0;
            int end   = (result.Value.Length - 20);

            var sha1Checksumns = new List<ReadOnlyMemory<byte>>();

            while (start <= end)
            {
                var sha1 = result.Value.Slice(start, 20);
                sha1Checksumns.Add(sha1);
                start += 20;
            }

            return new PieceHash(ImmutableList.CreateRange(sha1Checksumns));
        }

        private const string InfoMultiFileNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ReadDirectoryName(this BDictionary dict)
        {
            var infoDict = dict.GetValue<BDictionary>(RootInfoKey);

            if (infoDict.ContainsKey(InfoMultiFiles))
            {
                return infoDict.GetValue<BString>(InfoMultiFileNameKey).StringValue;
            }

            return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static FileUnitCollection ReadFiles(this BDictionary dict)
        {
            var infoDict = dict.GetValue<BDictionary>(RootInfoKey);

            if (infoDict.ContainsKey(InfoMultiFiles))
            {
                return infoDict.ReadMultiFiles();
            }

            var singleFile = infoDict.ReadSingleFile();

            var files = new List<FileUnit>() { singleFile };

            var collection = ImmutableList.CreateRange(files);

            return new FileUnitCollection(collection);
        }

        private const string InfoMultiFileNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadDirectoryName(this BDictionary dict)
        {
            var infoDict = dict.GetValue<BDictionary>(RootInfoKey);

            if (infoDict.ContainsKey(InfoMultiFiles))
            {
                return infoDict.GetValue<BString>(InfoMultiFileNameKey).StringValue;
            }

            return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FileUnitCollection ReadFiles(this BDictionary dict)
        {
            var infoDict = dict.GetValue<BDictionary>(RootInfoKey);

            if (infoDict.ContainsKey(InfoMultiFiles))
            {
                return infoDict.ReadMultiFiles();
            }

            var singleFile = infoDict.ReadSingleFile();

            var files = new List<FileUnit>() { singleFile };

            var collection = ImmutableList.CreateRange(files);

            return new FileUnitCollection(collection);
        }
    }
}