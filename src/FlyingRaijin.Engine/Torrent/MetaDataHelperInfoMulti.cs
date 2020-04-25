using FlyingRaijin.Bencode.Read.ClrObject;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    public static partial class MetaDataHelper
    {
        private static IImmutableList<MultiFileInfoDictionaryItem> EmptyFiles =
            ImmutableList.CreateRange(Enumerable.Empty<MultiFileInfoDictionaryItem>());

        private const string InfoMultiNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadMultiName(this BDictionary bDict)
            => bDict.GetValue<BString>(InfoMultiNameKey).Value;        

        private const string InfoMultiFiles = "files";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IImmutableList<MultiFileInfoDictionaryItem> ReadMultiFiles(this BDictionary bDict)
        {
            var files = bDict.GetValue<BList>(InfoMultiFiles).Value;

            if (files == null)
                return EmptyFiles;

            var typedFiles = new List<MultiFileInfoDictionaryItem>(files.Count);

            foreach (var fileDict in files.Cast<BDictionary>())
            {
                var item =
                    new MultiFileInfoDictionaryItem(
                        fileDict.ReadMultiFileLength(),
                        fileDict.ReadMultiFileM55Checksum(),
                        fileDict.ReadInfoMultiFilePath());

                typedFiles.Add(item);
            }

            return ImmutableList.CreateRange(typedFiles);
        }

        private const string InfoMultiFileLengthKey = "length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long ReadMultiFileLength(this BDictionary bDict)
            => bDict.GetValue<BInteger>(InfoMultiFileLengthKey).Value;

        private const string InfoMultiFileMd5ChecksumKey = "md5sum";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ReadMultiFileM55Checksum(this BDictionary bDict)
            => bDict.GetValue<BString>(InfoMultiFileMd5ChecksumKey).Value;

        private const string InfoMultiFilePathKey = "path";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ReadInfoMultiFilePath(this BDictionary bDict)
        {
            var list = bDict.GetValue<BList>(InfoMultiFilePathKey).Value;

            if (list == null)
                return string.Empty;

            if (list.Count == 1)
                return ((BString)list.First()).Value;

            return string.Join('/', list.Cast<BString>().Select(s => s.Value));
        }
    }
}