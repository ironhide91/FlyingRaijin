using System;
using System.Collections.Generic;

namespace FlyingRaijin.Client.Torrent
{
    public class MultiFileTorrent : ITorrent
    {
        //- ITorrent
        public Uri AnnounceUrl { get; private set; }

        public AnnounceList AnnounceList { get; private set; }

        public DateTime? CreationDate { get; private set; }

        public string Comment { get; private set; }

        public string CreatedBy { get; private set; }

        public string Encoding { get; private set; }

        public int PieceLength { get; private set; }

        public bool IsPrivate { get; private set; }

        //- Multiple File
        public string Name { get; private set; }

        public IReadOnlyList<MultipleFileItem> Files { get; private set; }
    }
}