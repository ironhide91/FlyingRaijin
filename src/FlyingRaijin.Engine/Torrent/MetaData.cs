using FlyingRaijin.Bencode.BObject;
using System;
using System.Collections.Immutable;

namespace FlyingRaijin.Engine.Torrent
{
    internal sealed class MetaData
    {
        internal readonly string AnnounceUrl;
        internal readonly AnnounceList AnnounceList;
        internal readonly DateTime CreationDate;
        internal readonly string Comment;
        internal readonly string CreatedBy;
        internal readonly string Encoding;

        internal readonly long PieceLength;
        internal readonly bool IsPrivate;
        internal readonly string Name;
        internal readonly PieceHash PieceHash;
        internal readonly FileUnitCollection Files;

        internal readonly InfoHash InfoHash;

        internal MetaData(BDictionary dictionary, InfoHash computedInfoHash)
        {
             AnnounceUrl = dictionary.ReadAnnounceUrl();
            AnnounceList = dictionary.ReadAnnounceList();
            CreationDate = dictionary.ReadCreationDate().FromUnixTime();
                 Comment = dictionary.ReadComment();
               CreatedBy = dictionary.ReadCreatedBy();
                Encoding = dictionary.ReadEncoding();

               PieceHash = dictionary.ReadPieceHash();
               IsPrivate = dictionary.ReadIsPrivateFlag();
             PieceLength = dictionary.ReadPieceLength();
                    Name = dictionary.ReadDirectoryName();
                   Files = dictionary.ReadFiles();

                InfoHash = computedInfoHash;
        }
    }
}