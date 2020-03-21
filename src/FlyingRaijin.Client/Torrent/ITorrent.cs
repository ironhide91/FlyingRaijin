using System;

namespace FlyingRaijin.Client.Torrent
{
    public interface ITorrent
    {
        Uri AnnounceUrl { get; }

        AnnounceList AnnounceList { get; }

        DateTime? CreationDate { get; }

        string Comment { get; }

        string CreatedBy { get; }

        string Encoding { get; }
    }
}