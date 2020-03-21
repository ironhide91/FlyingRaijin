using System.Collections.Generic;

namespace FlyingRaijin.Client.Torrent
{
    public sealed class AnnounceList
    {
        public AnnounceList(IReadOnlyList<IReadOnlyList<string>> annouceList)
        {
            Values = annouceList;
        }

        public readonly IReadOnlyList<IReadOnlyList<string>> Values;
    }
}