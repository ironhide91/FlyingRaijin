using System.Net;

namespace FlyingRaijin.Engine
{
    public class Peer
    {
        public readonly IPAddress IPAddress;
        public readonly ushort    Port;

        public Peer(IPAddress ipAddress, ushort port)
        {
            IPAddress = ipAddress;
                 Port = port;
        }

        public override string ToString()
        {
            return IPAddress.ToString() + " - " + Port.ToString();
        }
    }
}