using FlyingRaijin.Bencode.BObject;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Torrent
{
    internal static partial class MetaDataHelper
    {
        private const string InfoSingleNameKey = "name";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ReadSingleName(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoSingleNameKey);

            if (result == null)
                return string.Empty;

            return result.StringValue;
        }

        private const string InfoSingleLengthKey = "length";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static long ReadSingleLength(this BDictionary bDict)
        {
            var result = bDict.GetValue<BInteger>(InfoSingleLengthKey);

            if (result == null)
                return 0L;

            return result.Value;
        }

        private const string InfoSingleMD5ChecksumKey = "md5sum";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ReadSingleMD5Checksum(this BDictionary bDict)
        {
            var result = bDict.GetValue<BString>(InfoSingleMD5ChecksumKey);

            if (result == null)
                return string.Empty;

            return result.StringValue;
        }

        internal static FileUnit ReadSingleFile(this BDictionary infoDict)
        {
            var length = infoDict.ReadSingleLength();

            var file = new FileUnit(
                     infoDict.ReadSingleName(),
                     infoDict.ReadSingleMD5Checksum(),
                     length,
                     0,
                     length-1);

            return file;
        }
    }
}