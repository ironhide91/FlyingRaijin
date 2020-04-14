using FlyingRaijin.Bencode.Read.ClrObject;
using System;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class MultiFileTorrent : TorrentBase<MultiFileInfoDictionary>
    {
        public MultiFileTorrent(BDictionary dictionary)
        {
             _AnnounceUrl = dictionary.ReadAnnounceUrl();
            _AnnounceList = dictionary.ReadAnnounceList();
            _CreationDate = dictionary.ReadCreationDate().FromUnixTime();
                 _Comment = dictionary.ReadComment();
               _CreatedBy = dictionary.ReadCreatedBy();
                _Encoding = dictionary.ReadEncoding();
                    _Info = dictionary.ReadMultiFileInfoDictionary();
        }

        public override string AnnounceUrl { get { return _AnnounceUrl; } }
        public override AnnounceList AnnounceList { get { return _AnnounceList; } }
        public override DateTime CreationDate { get { return _CreationDate; } }
        public override string Comment { get { return _Comment; } }
        public override string CreatedBy { get { return _CreatedBy; } }
        public override string Encoding { get { return _Encoding; } }
        public override MultiFileInfoDictionary Info { get { return _Info; } }

        private readonly string _AnnounceUrl;
        private readonly AnnounceList _AnnounceList;
        private readonly DateTime _CreationDate;
        private readonly string _Comment;
        private readonly string _CreatedBy;
        private readonly string _Encoding;
        private readonly MultiFileInfoDictionary _Info;
    }
}