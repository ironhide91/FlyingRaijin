using FlyingRaijin.Bencode.BObject;
using System;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class MultiFileTorrent : TorrentBase<MultiFileInfoDictionary>
    {
        public MultiFileTorrent(BDictionary _dictionary, ReadOnlyMemory<byte> _infoHash)
        {
             announceUrl = _dictionary.ReadAnnounceUrl();
            announceList = _dictionary.ReadAnnounceList();
            creationDate = _dictionary.ReadCreationDate().FromUnixTime();
                 comment = _dictionary.ReadComment();
               createdBy = _dictionary.ReadCreatedBy();
                encoding = _dictionary.ReadEncoding();
                    info = _dictionary.ReadMultiFileInfoDictionary();
                infoHash = _infoHash;
        }

        public override string AnnounceUrl { get { return announceUrl; } }
        public override AnnounceList AnnounceList { get { return announceList; } }
        public override DateTime CreationDate { get { return creationDate; } }
        public override string Comment { get { return comment; } }
        public override string CreatedBy { get { return createdBy; } }
        public override string Encoding { get { return encoding; } }
        public override MultiFileInfoDictionary Info { get { return info; } }
        public override ReadOnlyMemory<byte> InfoHash { get { return infoHash; } }

        private readonly string announceUrl;
        private readonly AnnounceList announceList;
        private readonly DateTime creationDate;
        private readonly string comment;
        private readonly string createdBy;
        private readonly string encoding;
        private readonly MultiFileInfoDictionary info;
        private readonly ReadOnlyMemory<byte> infoHash;
    }
}