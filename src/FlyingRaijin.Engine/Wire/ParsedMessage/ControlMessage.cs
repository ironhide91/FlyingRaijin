namespace FlyingRaijin.Engine.Wire
{
    internal class ControlMessage : IControlMessage
    {
        internal static readonly ControlMessage Choke =
            new ControlMessage(MessageId.Choke);

        internal static readonly ControlMessage Unchoke =
            new ControlMessage(MessageId.Unchoke);

        internal static readonly ControlMessage Interested =
            new ControlMessage(MessageId.Interested);

        internal static readonly ControlMessage NotInterested =
            new ControlMessage(MessageId.NotInterested);

        internal static readonly ControlMessage Cancel =
            new ControlMessage(MessageId.Cancel);

        private ControlMessage(MessageId id)
        {
            MessageId = id;
        }

        public MessageId MessageId { get; private set; }
    }
}