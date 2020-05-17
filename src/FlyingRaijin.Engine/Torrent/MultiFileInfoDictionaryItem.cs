using System;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class MultiFileInfoDictionaryItem : IEquatable<MultiFileInfoDictionaryItem>
    {
        public MultiFileInfoDictionaryItem(long lengthInBytes, string md5Checksum, string path)
        {
            LengthInBytes = lengthInBytes;
              Md5Checksum = md5Checksum;
                     Path = path;
        }

        public bool Equals(MultiFileInfoDictionaryItem other)
        {
            return Path.Equals(other?.Path);
        }

        public readonly long LengthInBytes;
        public readonly string Md5Checksum;
        public readonly string Path;
    }
}