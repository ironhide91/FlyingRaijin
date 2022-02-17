namespace FlyingRaijin.Engine.Torrent
{
    internal class FileUnit
    {                
        internal readonly string Path;
        internal readonly string Md5Checksum;
        internal readonly long LengthInBytes;        
        internal readonly long PieceIndexBegin;
        internal readonly long PieceIndexEnd;
        
        internal FileUnit(
            string path,
            string md5Checksum,
            long lengthInBytes,
            long pieceIndexBegin,
            long pieceIndexEnd)
        {
            Path = path;
            Md5Checksum = md5Checksum;
            LengthInBytes = lengthInBytes;
            PieceIndexBegin = pieceIndexBegin;
            PieceIndexEnd = pieceIndexEnd;
        }

        public bool Equals(FileUnit other)
        {
            return Path.Equals(other?.Path);
        }
    }
}