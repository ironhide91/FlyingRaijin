namespace FlyingRaijin.Engine.Messages.Peer
{
    internal sealed class PeerConnectionClosedMessage
    {
        internal static readonly PeerConnectionClosedMessage Instance = new PeerConnectionClosedMessage();

        private PeerConnectionClosedMessage()
        {

        }
    }
}