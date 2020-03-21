using System;
using System.Collections.Generic;
using FlyingRaijin.Bencode.Read.ClrObject;

namespace FlyingRaijin.Client.Torrent
{
    public static class MetaInfoHelper
    {
        //- Root level keys
        private const string                 ROOT_INFO = "info";
        private const string         ROOT_ANNOUNCE_URL = "announce";
        private const string        ROOT_ANNOUNCE_LIST = "announce-list";
        private const string        ROOT_CREATION_DATE = "creation date";
        private const string              ROOT_COMMENT = "comment";
        private const string           ROOT_CREATED_BY = "created by";
        private const string             ROOT_ENCODING = "encoding";
        //- Info Dictionary Keys
        private const string     INFO_PIECE_LENGTH_KEY = "piece length";
        private const string           INFO_PIECES_KEY = "pieces";
        private const string          INFO_PRIVATE_KEY = "private";
        //- Info Dictionary Single File Keys
        private const string          INFO_SINGLE_NAME = "name";
        private const string        INFO_SINGLE_LENGTH = "length";
        private const string        INFO_SINGLE_MD5SUM = "md5sum";
        //- Info Dictionary Multi File Keys
        private const string        INFO_MULTIPLE_NAME = "name";
        private const string       INFO_MULTIPLE_FILES = "files";
        //- Info Dictionary Multi File Item Keys
        private const string INFO_MULTIPLE_ITEM_LENGTH = "length";
        private const string INFO_MULTIPLE_ITEM_MD5SUM = "md5sum";
        private const string   INFO_MULTIPLE_ITEM_PATH = "path";

        //- Required
        public static string ReadAnnounceUrl(this in BDictionary bDict)
        {
            return bDict.GetValue<string>(ROOT_ANNOUNCE_URL);
        }

        public static int ReadPieceLength(this BDictionary bDict)
        {
            return bDict.GetValue<int>(INFO_PIECE_LENGTH_KEY);
        }

        public static bool ReadPrivateFlag(this BDictionary bDict)
        {
            return (bDict.GetValue<int>(INFO_PRIVATE_KEY) == 1);
        }

        public static IReadOnlyList<string> ReadPieces(this BDictionary bDict)
        {
            return null;
        }

        //- Optional        
        public static long ReadCreationDate(this BDictionary bDict)
        {
            return bDict.TryGetValue<long>(ROOT_CREATION_DATE);
        }

        public static string ReadComment(this BDictionary bDict)
        {
            return bDict.TryGetValue<string>(ROOT_COMMENT);
        }

        public static string ReadCreatedBy(this BDictionary bDict)
        {
            return bDict.TryGetValue<string>(ROOT_CREATED_BY);
        }

        public static string ReadEncoding(this BDictionary bDict)
        {
            return bDict.TryGetValue<string>(ROOT_ENCODING);
        }
    }
}