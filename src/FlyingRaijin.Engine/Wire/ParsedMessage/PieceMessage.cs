using System;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PieceMessage : IMessage
    {
        public MessageId MessageId { get { return MessageId.Piece; } }

        /// <summary>
        /// integer specifying the zero-based piece index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// integer specifying the zero-based byte offset within the piece
        /// </summary>
        public int Begin { get; set; }

        /// <summary>
        /// block of data, which is a subset of the piece specified by index
        /// </summary>
        public ReadOnlyMemory<byte> Data { get; set; }
    }
}