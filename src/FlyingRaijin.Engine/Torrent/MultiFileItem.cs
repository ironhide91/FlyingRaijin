using FlyingRaijin.Bencode.Read.ClrObject;
using FlyingRaijin.Bencode.Read;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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