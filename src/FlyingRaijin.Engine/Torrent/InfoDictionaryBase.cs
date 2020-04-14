using System.Collections.Immutable;

namespace FlyingRaijin.Engine.Torrent
{
    public abstract class InfoDictionaryBase
    {
        public abstract long PieceLength { get; }
        public abstract ImmutableList<string> Pieces { get; }
        public abstract bool IsPrivate { get; }        
    }
}