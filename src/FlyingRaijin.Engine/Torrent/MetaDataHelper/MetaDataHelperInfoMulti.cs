using FlyingRaijin.Bencode.BObject;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    internal static partial class MetaDataHelper
    {
        private const string InfoMultiNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ReadMultiName(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoMultiNameKey);

            if (result == null)
                return string.Empty;

            return result.StringValue;
        }

        internal const string InfoMultiFiles = "files";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static FileUnitCollection ReadMultiFiles(this BDictionary bDict)
        {
            var files = bDict.GetValue<BList>(InfoMultiFiles);

            if (files == null)
                return FileUnitCollection.Empty;

            var typedFiles = new List<FileUnit>(files.Value.Count);

            long index = 0;

            foreach (var fileDict in files.Value.Cast<BDictionary>())
            {
                var length = fileDict.ReadMultiFileLength();
                long end = (index + length -1);

                var item = new FileUnit(
                    fileDict.ReadInfoMultiFilePath(),
                    fileDict.ReadMultiFileM55Checksum(),
                    fileDict.ReadMultiFileLength(),
                    index,
                    end);

                typedFiles.Add(item);
                index = end + 1;
            }

            var collection = ImmutableList.CreateRange(typedFiles);

            return new FileUnitCollection(collection);
        }

        private const string InfoMultiFileLengthKey = "length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long ReadMultiFileLength(this BDictionary bDict)
        {
            var result = bDict.GetValue<BInteger>(InfoMultiFileLengthKey);

            if (result == null)
                return 0L;

            return result.Value;
        }

        private const string InfoMultiFileMd5ChecksumKey = "md5sum";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ReadMultiFileM55Checksum(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoMultiFileMd5ChecksumKey);

            if (result == null)
                return string.Empty;

            return result.StringValue;
        }

        private const string InfoMultiFilePathKey = "path";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ReadInfoMultiFilePath(this BDictionary bDict)
        {
            var list = bDict.GetValue<BList>(InfoMultiFilePathKey);

            if (list == null)
                return string.Empty;

            if (list.Value.Count == 1)
                return ((BString)list.Value.First()).StringValue;

            return string.Join('/', list.Value.Cast<BString>().Select(s => s.StringValue));
        }
    }
}