namespace FlyingRaijin.Engine.Torrent
{
    public abstract class InfoDictionaryBase
    {
        public abstract long PieceLength { get; }
        public abstract PieceHash Pieces { get; }
        public abstract bool IsPrivate { get; }
    }
}