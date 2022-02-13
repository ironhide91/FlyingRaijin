using System.Buffers;

namespace FlyingRaijin.Engine.Wire
{
    internal class PieceBlock : ByteBufferManager
    {
        public PieceBlock()
        {

        }

        private IMemoryOwner<byte> buffer;

        internal int PendingPieceLength { get; set; }

        internal void SetPendingPieceLength(int pieceLength)
        {
            PendingPieceLength = pieceLength;
        }

        internal void Add(PieceMessage message)
        {
            var block = buffer.Memory.Span.Slice(message.Begin, message.Block.Length);
            message.Block.Span.CopyTo(block);

            PendingPieceLength -= message.Block.Length;

            PieceMessagePool.Pool.Return(message);
        }
    }
}