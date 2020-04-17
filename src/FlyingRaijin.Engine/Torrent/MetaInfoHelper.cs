using FlyingRaijin.Bencode.Read.ClrObject;
using SimpleBase;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlyingRaijin.Engine.Torrent
{
    public static class MetaInfoHelper
    {
        private static readonly Base16 Base16 = new Base16(Base16Alphabet.UpperCase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T GetValue<T>(this BDictionary dictionary, string key) where T : IClrObject
        {
            T value = default;

            try
            {
                value = (T)dictionary.Value[key];
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return value;
        }

        private const string RootInfoKey = "info";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BDictionary ReadInfo(this BDictionary bDict)
        {
            return bDict.GetValue<BDictionary>(RootInfoKey);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SingleFileInfoDictionary ReadSingleFileInfoDictionary(this BDictionary bDict)
        {
                 var info = bDict.GetValue<BDictionary>(RootInfoKey);
            var typedInfo = new SingleFileInfoDictionary(info);

            return typedInfo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MultiFileInfoDictionary ReadMultiFileInfoDictionary(this BDictionary bDict)
        {
            var info = bDict.GetValue<BDictionary>(RootInfoKey);
            var typedInfo = new MultiFileInfoDictionary(info);

            return typedInfo;
        }

        private const string RootAnnounceUrlKey = "announce";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadAnnounceUrl(this BDictionary bDict)
            => bDict.GetValue<BString>(RootAnnounceUrlKey).Value;        

        private const string RootAnnounceListKey = "announce-list";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AnnounceList ReadAnnounceList(this BDictionary bDict)
        {
            var bList = bDict.GetValue<BList>(RootAnnounceListKey);          

            if (bList.Value == null)
                return AnnounceList.Empty;

            var levelOneAreNotBlist = bList.Value.Any(x => x.GetType() != typeof(BList));

            var levelTwoAreNotBstring = bList.Value.Cast<BList>().Any(one => one.Value.Any(two => two.GetType() != typeof(BString)));

            if (levelOneAreNotBlist || levelTwoAreNotBstring)
                return AnnounceList.Empty;

            var tiers = new List<IImmutableList<string>>();

            foreach (BList tier in bList.Value)
            {
                var trackers = new List<string>();

                foreach (BString tracker in tier.Value)
                    trackers.Add(tracker.Value);

                if (trackers.Count > 0)
                    tiers.Add(ImmutableList.CreateRange(trackers));
            }

            IImmutableList<IImmutableList<string>> announceList = ImmutableList.CreateRange(tiers);

            return new AnnounceList(announceList);
        }

        private const string RootCreationDateKey = "creation date";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadCreationDate(this BDictionary bDict)
            => bDict.GetValue<BInteger>(RootCreationDateKey).Value;

        private const string RootCommentKey = "comment";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadComment(this BDictionary bDict)
            => bDict.GetValue<BString>(RootCommentKey).Value;

        private const string RootCreatedByKey = "created by";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadCreatedBy(this BDictionary bDict)
            => bDict.GetValue<BString>(RootCreatedByKey).Value;

        private const string RootEncodingKey = "encoding";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadEncoding(this BDictionary bDict)
            => bDict.GetValue<BString>(RootEncodingKey).Value;

        //-> Common Info Dictionary
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

        //-> Single File Info Dictionary
        private const string InfoSingleNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadSingleName(this BDictionary bDict)
            => bDict.GetValue<BString>(InfoSingleNameKey).Value;

        private const string InfoSingleLengthKey = "length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadSingleLength(this BDictionary bDict)
            => bDict.GetValue<BInteger>(InfoSingleLengthKey).Value;

        private const string InfoSingleMD5ChecksumKey = "md5sum";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadSingleMD5Checksum(this BDictionary bDict)
            => bDict.GetValue<BString>(InfoSingleMD5ChecksumKey).Value;

        //-> Multi File Info Dictionary
        private const string InfoMultiNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadMultiName(this BDictionary bDict)
            => bDict.GetValue<BString>(InfoMultiNameKey).Value;

        private const string InfoMultiFiles = "files";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableList<MultiFileInfoDictionaryItem> ReadMultiFiles(this BDictionary bDict)
        {
            var files = bDict.GetValue<BList>(InfoMultiFiles).Value;

            var typedFiles = new List<MultiFileInfoDictionaryItem>(files.Count);

            foreach (var fileDict in files.Cast<BDictionary>())
            {
                var item =
                    new MultiFileInfoDictionaryItem(
                        fileDict.ReadMultiFileLength(),
                        fileDict.ReadInfoMultiFilePath(),
                        fileDict.ReadMultiFileM55Checksum());

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

            if (list.Count == 1)
            {
                return ((BString)list.First()).Value;
            }

            return string.Join('/', list.Cast<BString>().Select(s => s.Value));
        }        
    }
}