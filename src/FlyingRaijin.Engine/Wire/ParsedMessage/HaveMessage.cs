namespace FlyingRaijin.Engine.Wire
{
    internal sealed class HaveMessage : IMessage
    {
        public const long MessageLength = 4;

        public MessageId MessageId { get { return MessageId.Have; } }

        public int Index { get; set; }
    }
}