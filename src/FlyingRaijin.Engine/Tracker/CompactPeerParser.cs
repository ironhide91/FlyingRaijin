using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;

namespace FlyingRaijin.Engine.Tracker
{
    public static class CompactPeerParser
    {
        private const int length = 6;

        public static bool TryParsePeers(ReadOnlySpan<byte> data, out IList<DnsEndPoint> peers)
        {
            peers = default;

            if (data == null || ((data.Length % 6) != 0))
                return false;

            peers = new List<DnsEndPoint>(data.Length/6);

            int index = 0;

            while (index < data.Length)
            {
                DnsEndPoint peer;

                if (TryParsePeer(data.Slice(index, length), out peer))
                {
                    peers.Add(peer);
                }

                index += 6;
            }

            return true;
        }

        private static bool TryParsePeer(ReadOnlySpan<byte> data, out DnsEndPoint peer)
        {
            peer = default;

            if (data == null || data.Length != length)
                return false;            

            // First 4 bytes represent the IP Address.
            var ip = new IPAddress(data.Slice(0, 4));

            // Last 2 bytes represent the Port in Big Endian.
            var portData = data.Slice(4, 2).ToArray().AsSpan();
            portData.Reverse();
            var port = BitConverter.ToUInt16(portData);

            peer = new DnsEndPoint(ip.ToString(), port);

            //var buffer = data.Slice(4).ToArray();
            //Array.Reverse(buffer);
            //var temp = BitConverter.ToUInt16(buffer.AsSpan());

            return true;
        }
    }
}