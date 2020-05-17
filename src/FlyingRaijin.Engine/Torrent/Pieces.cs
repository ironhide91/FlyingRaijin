using System.Collections.Immutable;
using System.Linq;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class Pieces
    {
        public static readonly Pieces Empty =
            new Pieces(ImmutableList.CreateRange(Enumerable.Empty<byte[]>()));

        public Pieces(IImmutableList<byte[]> sha1Checksums)
        {
            Sha1Checksums = sha1Checksums;
        }

        public readonly IImmutableList<byte[]> Sha1Checksums;
    }
}