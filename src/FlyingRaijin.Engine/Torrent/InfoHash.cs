using System;

namespace FlyingRaijin.Engine.Torrent
{
    internal class InfoHash
    {
        internal InfoHash(ReadOnlyMemory<byte> hash)
        {
            Hash = hash;
        }

        public readonly ReadOnlyMemory<byte> Hash;
    }
}