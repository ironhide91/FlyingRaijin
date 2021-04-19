using System.Collections.Generic;
using System.Net;

namespace FlyingRaijin.Engine.Messages
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