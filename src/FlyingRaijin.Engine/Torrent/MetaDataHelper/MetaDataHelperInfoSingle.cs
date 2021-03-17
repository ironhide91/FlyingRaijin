using FlyingRaijin.Bencode.BObject;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    public static partial class MetaDataHelper
    {
        private const string InfoSingleNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadSingleName(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoSingleNameKey);

            if (result == null)
                return string.Empty;

            return result.StringValue;
        }

        private const string InfoSingleLengthKey = "length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadSingleLength(this BDictionary bDict)
        {
            var result = bDict.GetValue<BInteger>(InfoSingleLengthKey);

            if (result == null)
                return 0L;

            return result.Value;
        }

        private const string InfoSingleMD5ChecksumKey = "md5sum";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadSingleMD5Checksum(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoSingleMD5ChecksumKey);

            if (result == null)
                return string.Empty;

            return result.StringValue;
        }
    }
}