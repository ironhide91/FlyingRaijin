using FlyingRaijin.Bencode.Read.ClrObject;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    public static partial class MetaDataHelper
    {
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
    }
}