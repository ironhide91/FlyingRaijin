using System.Collections.Generic;
using System.Net;

namespace FlyingRaijin.Messages
{
    public class NewPeers
    {
        public readonly IEnumerable<DnsEndPoint> Peers;

        public NewPeers(IEnumerable<DnsEndPoint> peers)
        {
            Peers = peers;
        }
    }
}