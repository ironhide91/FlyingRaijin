using System;
using System.Collections.Immutable;
using System.Linq;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class PieceHash
    {
        public static readonly PieceHash Empty =
            new PieceHash(ImmutableList.CreateRange(Enumerable.Empty<ReadOnlyMemory<byte>>()));

        public PieceHash(IImmutableList<ReadOnlyMemory<byte>> sha1Checksums)
        {
            Sha1Checksums = sha1Checksums;
        }

        public readonly IImmutableList<ReadOnlyMemory<byte>> Sha1Checksums;
    }
}