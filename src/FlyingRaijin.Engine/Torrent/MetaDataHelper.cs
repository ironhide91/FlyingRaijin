using FlyingRaijin.Bencode.Read.ClrObject;
using SimpleBase;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    public static partial class MetaDataHelper
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

    }
}