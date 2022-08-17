using FlyingRaijin.Bencode.BObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlyingRaijin.Engine.Torrent
{
    internal static partial class MetaDataHelper
    {
        private static readonly ReadOnlyDictionary<string, BString> KnownKeys =
            new ReadOnlyDictionary<string, BString>(
                new Dictionary<string, BString>()
                {
                    { RootInfoKey, new BString(null, RootInfoKey.ToBytes()) },
                    { RootAnnounceUrlKey, new BString(null, RootAnnounceUrlKey.ToBytes()) },
                    { RootAnnounceListKey, new BString(null, RootAnnounceListKey.ToBytes()) },
                    { RootCreationDateKey, new BString(null, RootCreationDateKey.ToBytes()) },
                    { RootCommentKey, new BString(null, RootCommentKey.ToBytes()) },
                    { RootCreatedByKey, new BString(null, RootCreatedByKey.ToBytes()) },
                    { RootEncodingKey, new BString(null, RootEncodingKey.ToBytes()) },
                    { InfoPieceLengthKey, new BString(null, InfoPieceLengthKey.ToBytes()) },
                    { InfoPrivateKey, new BString(null, InfoPrivateKey.ToBytes()) },
                    { InfoPiecesKey, new BString(null, InfoPiecesKey.ToBytes()) },
                    { InfoSingleNameKey, new BString(null, InfoSingleNameKey.ToBytes()) },
                    { InfoSingleLengthKey, new BString(null, InfoSingleLengthKey.ToBytes()) },
                    { InfoSingleMD5ChecksumKey, new BString(null, InfoSingleMD5ChecksumKey.ToBytes()) },
                    { InfoMultiFilePathKey, new BString(null, InfoMultiFilePathKey.ToBytes()) },
                    { InfoMultiFiles, new BString(null, InfoMultiFiles.ToBytes()) },                    
                });

        internal static ReadOnlyMemory<byte> ToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}