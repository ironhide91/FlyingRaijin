using System;

namespace FlyingRaijin.Engine.Wire
{
    internal class PieceBlock : ByteBufferManager
    {
        public PieceBlock()
        {
            
        }

        internal int Index
        {
            get
            {
                return index.Value;
            }
        }

        internal bool IsPending
        {
            get
            {
                return pendingPieceLength.HasValue && pendingPieceLength.Value == 0;
            }
        }

        internal void SetPieceIndex(int index)
        {
            this.index = index;
        }

        internal void SetPendingPieceLength(int pendingBytes)
        {
            pendingPieceLength = pendingBytes;
        }

        internal void ResetIndex()
        {
            index = null;
        }

        internal void ResetPendingPieceLength()
        {
            pendingPieceLength = null;
        }

        internal void Write(PieceMessage message, ReadOnlySpan<byte> source)
        {
            var destination = buffer.Memory.Span.Slice(message.Begin, message.Length);
            source.CopyTo(destination);

            pendingPieceLength =- message.Length;
        }

        private int? index;
        private int? pendingPieceLength;
    }
}