using System;

namespace FlyingRaijin.Engine.Messages
{
    public class FileRead
    {
        public readonly ReadOnlyMemory<byte> Data;

        public FileRead(ReadOnlyMemory<byte> data)
        {
            Data = data;
        }
    }
}