using System;
using System.Collections.Generic;
using System.Net;

namespace FlyingRaijin.Engine.Tracker
{
    public static class CompactPeerParser
    {
        private const int length = 6;

        public static bool TryParsePeers(ReadOnlySpan<byte> data, out IList<Peer> peers)
        {
            peers = default;

            if (data == null || ((data.Length % 6) != 0))
                return false;

            peers = new List<Peer>(data.Length/6);

            int index = 0;

            while (index < data.Length)
            {
                Peer peer;

                if (TryParsePeer(data.Slice(index, length), out peer))
                {
                    peers.Add(peer);
                }

                index += 6;
            }

            return true;
        }

        private static bool TryParsePeer(ReadOnlySpan<byte> data, out Peer peer)
        {
            peer = default;

            if (data == null || data.Length != length)
                return false;

            // First 4 bytes represent the IP Address
            var ip = new IPAddress(data.Slice(0, 4));

            // Laat 2 bytes represent the Port
            var port = BitConverter.ToUInt16(data.Slice(4, 2));

            peer = new Peer(ip, port);

            return true;
        }
    }
}