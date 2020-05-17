using System;
using System.Collections.Immutable;
using System.Linq;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class AnnounceList
    {
        public static readonly AnnounceList Empty =
            new AnnounceList(ImmutableList.CreateRange(Enumerable.Empty<IImmutableList<string>>()));

        public AnnounceList(IImmutableList<IImmutableList<string>> annouceList)
        {
            Tiers = annouceList;
        }

        public readonly IImmutableList<IImmutableList<string>> Tiers;
    }
}