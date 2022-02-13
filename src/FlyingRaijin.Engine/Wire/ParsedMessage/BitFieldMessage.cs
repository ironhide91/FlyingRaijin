namespace FlyingRaijin.Engine.Wire
{
    internal sealed class BitFieldMessage : ByteBufferManager, IMessage
    {
        public MessageId MessageId { get { return MessageId.BitField; } }

        /// <summary>
        /// integer specifying the zero-based piece index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// integer specifying the zero-based byte offset within the piece
        /// </summary>
        public int Begin { get; set; }

        /// <summary>
        /// integer specifying the requested length.
        /// </summary>
        public int Length { get; set; }
    }
}