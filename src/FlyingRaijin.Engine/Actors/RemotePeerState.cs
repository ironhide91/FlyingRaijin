namespace FlyingRaijin.Engine.Actors
{
    public partial class PeerActor
    {
        enum RemotePeerState
        {           
            Failed,
            Closed,
            Disconnected,
            Connecting,
            Connected,
            HandshakeInProgress,
            HandshakeSuccessfull,
            HandshakeUnsuccessfull,
            Payload
        }
    }
}