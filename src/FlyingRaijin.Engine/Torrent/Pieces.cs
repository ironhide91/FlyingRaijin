using System;
using System.Collections.Immutable;
using System.Linq;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class Pieces
    {
        public static readonly Pieces Empty =
            new Pieces(ImmutableList.CreateRange(Enumerable.Empty<ReadOnlyMemory<byte>>()));

        public Pieces(IImmutableList<ReadOnlyMemory<byte>> sha1Checksums)
        {
            Sha1Checksums = sha1Checksums;
        }

        public readonly IImmutableList<ReadOnlyMemory<byte>> Sha1Checksums;
    }
}