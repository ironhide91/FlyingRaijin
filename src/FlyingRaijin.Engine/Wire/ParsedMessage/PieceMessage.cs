namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PieceMessage : IMessage
    {
        public PieceMessage()
        {

        }

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
        /// length of the block
        /// </summary>
        public int Length { get; set; }
    }
}