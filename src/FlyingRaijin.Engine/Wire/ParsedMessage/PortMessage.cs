namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PortMessage : IMessage
    {
        public const long MessageLength = 2;

        public MessageId MessageId { get { return MessageId.Port; } }

        public ushort Port { get; set; }
    }
}