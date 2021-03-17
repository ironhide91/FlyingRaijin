using System;

namespace FlyingRaijin.Engine.Torrent
{
    public abstract class TorrentBase<T> where T : InfoDictionaryBase
    {
        public abstract string AnnounceUrl { get; }

        public abstract AnnounceList AnnounceList { get; }

        public abstract DateTime CreationDate { get; }

        public abstract string Comment { get; }

        public abstract string CreatedBy { get; }

        public abstract string Encoding { get; }

        public abstract T Info { get; }

        public abstract ReadOnlyMemory<byte> InfoHash { get; }

    }
}