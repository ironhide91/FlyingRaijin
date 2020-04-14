using System;
using System.Collections.Immutable;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class AnnounceList
    {
        public AnnounceList(ImmutableList<ImmutableList<string>> annouceList)
        {
            Tiers = annouceList;
        }

        public readonly ImmutableList<ImmutableList<string>> Tiers;
    }
}