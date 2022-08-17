namespace FlyingRaijin.Engine.Wire
{
    internal sealed class RequestMessage : IMessage
    {
        public const long MessageLength = 12;

        public MessageId MessageId { get { return MessageId.Request; } }

        /// <summary>
        /// integer specifying the zero-based piece index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// integer specifying the zero-based byte offset within the piece
        /// </summary>
        public int Begin { get; set; }

        /// <summary>
        /// integer specifying the requested length
        /// </summary>
        public int Length { get; set; }
    }
}