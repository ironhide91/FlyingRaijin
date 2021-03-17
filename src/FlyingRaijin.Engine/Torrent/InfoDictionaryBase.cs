namespace FlyingRaijin.Engine.Torrent
{
    public abstract class InfoDictionaryBase
    {
        public abstract long PieceLength { get; }
        public abstract Pieces Pieces { get; }
        public abstract bool IsPrivate { get; }
    }
}