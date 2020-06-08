using System;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class MultiFileItem
    {
        public MultiFileItem()
        {

        }

        private readonly Uri File;

        public string Path { get { return File.AbsolutePath; } }

        //public string Extension { get { return File.ex; } }

        public readonly int LengthInBytes;
    }
}