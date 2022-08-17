using System;
using System.Buffers;

namespace FlyingRaijin.Engine
{
    internal class MemorySegment<T> : ReadOnlySequenceSegment<T>
    {
        internal MemorySegment(ReadOnlyMemory<T> memory)
        {
            Memory = memory;
        }

        internal MemorySegment<T> Append(ReadOnlyMemory<T> memory)
        {
            var segment = new MemorySegment<T>(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };

            Next = segment;

            return segment;
        }
    }
}