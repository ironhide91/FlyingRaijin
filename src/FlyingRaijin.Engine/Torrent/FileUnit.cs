namespace FlyingRaijin.Engine.Torrent
{
    public class FileUnit
    {
        public readonly long   LengthInBytes;
        public readonly string Md5Checksum;
        public readonly string Path;

        public FileUnit(long lengthInBytes, string md5Checksum, string path)
        {
            LengthInBytes = lengthInBytes;
              Md5Checksum = md5Checksum;
                     Path = path;
        }

        public bool Equals(FileUnit other)
        {
            return Path.Equals(other?.Path);
        }
    }
}