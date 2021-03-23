using FlyingRaijin.Bencode.BObject;
using System;
using System.Collections.Immutable;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class MetaData
    {
        public readonly string             AnnounceUrl;
        public readonly AnnounceList       AnnounceList;
        public readonly DateTime           CreationDate;
        public readonly string             Comment;
        public readonly string             CreatedBy;
        public readonly string             Encoding;
                                           
        public readonly long               PieceLength;
        public readonly bool               IsPrivate;        
        public readonly string             Name;
        public readonly Pieces             Pieces;
        public readonly FileUnitCollection Files;

        public readonly ReadOnlyMemory<byte>   InfoHash;

        public MetaData(BDictionary dictionary, ReadOnlyMemory<byte> computedInfoHash)
        {
             AnnounceUrl = dictionary.ReadAnnounceUrl();
            AnnounceList = dictionary.ReadAnnounceList();
            CreationDate = dictionary.ReadCreationDate().FromUnixTime();
                 Comment = dictionary.ReadComment();
               CreatedBy = dictionary.ReadCreatedBy();
                Encoding = dictionary.ReadEncoding();

                  Pieces = dictionary.ReadPieces();
               IsPrivate = dictionary.ReadIsPrivateFlag();
             PieceLength = dictionary.ReadPieceLength();
                    Name = dictionary.ReadDirectoryName();
                   Files = dictionary.ReadFiles();

                InfoHash = computedInfoHash;
        }
    }
}