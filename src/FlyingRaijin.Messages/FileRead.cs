using System;

namespace FlyingRaijin.Messages
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