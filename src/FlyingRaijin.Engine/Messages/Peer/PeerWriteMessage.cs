using System;

namespace FlyingRaijin.Engine.Messages.Peer
{
    internal class PeerWriteMessage
    {
        internal readonly ReadOnlyMemory<byte> Data;

        internal static PeerWriteMessage Create(ReadOnlyMemory<byte> data)
        {
            return new PeerWriteMessage(data);
        }

        private PeerWriteMessage(ReadOnlyMemory<byte> data)
        {
            Data = data;
        }
    }
}