using System;

namespace FlyingRaijin.Engine.Messages.Peer
{
    internal class PeerWriteMessage
    {
        internal static readonly PeerWriteMessage Empty =
            new PeerWriteMessage(ReadOnlyMemory<byte>.Empty);

        internal static PeerWriteMessage Create(ReadOnlyMemory<byte> data)
        {
            return new PeerWriteMessage(data);
        }

        internal readonly ReadOnlyMemory<byte> Data;

        private PeerWriteMessage(ReadOnlyMemory<byte> data)
        {
            Data = data;
        }
    }
}