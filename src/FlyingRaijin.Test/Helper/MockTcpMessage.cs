using Akka.IO;

namespace FlyingRaijin.Test
{
    internal static class MockTcpMessage
    {
        internal static readonly Tcp.CommandFailed Failed = new Tcp.CommandFailed(null);
        internal static readonly Tcp.Connected Connected = new Tcp.Connected(null, null);
        internal static readonly Tcp.ConnectionClosed Closed = new Tcp.ConnectionClosed();
        internal static readonly Tcp.Received Received = new Tcp.Received(null);
    }
}