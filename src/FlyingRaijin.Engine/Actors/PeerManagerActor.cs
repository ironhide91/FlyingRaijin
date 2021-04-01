using Akka.Actor;
using Akka.IO;
using Akka.Streams.Dsl;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Tracker;
using FlyingRaijin.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;
using IoTcp = Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Actors
{
    public class PeerManagerActor : ReceiveActor
    {        
        private static readonly ReadOnlyMemory<byte> protocolIdentifierLength;
        private static readonly ReadOnlyMemory<byte> protocolIdentifier;        
        private static readonly ReadOnlyMemory<byte> reservedBytes = new byte[8];
        private static readonly MemorySegment<byte>  handShakeHeader;
        private static readonly MemorySegment<byte>  handShakeTrail;

        private readonly MetaData torrent;
        private readonly HashSet<DnsEndPoint> peers;

        static PeerManagerActor()
        {
                  protocolIdentifier = Encoding.UTF8.GetBytes("BitTorrent protocol");
            protocolIdentifierLength = new byte[] { (byte)protocolIdentifier.Length };

            handShakeHeader = new MemorySegment<byte>(protocolIdentifierLength);

            handShakeTrail = handShakeHeader
                .Append(protocolIdentifier)
                .Append(reservedBytes);
        }

        public static Props Props(MetaData torrent, IEnumerable<DnsEndPoint> peers)
        {
            return Akka.Actor.Props.Create(() => new PeerManagerActor(torrent, peers));
        }

        public PeerManagerActor(MetaData torrent, IEnumerable<DnsEndPoint> peers)
        {
             this.torrent = torrent;
               this.peers = new HashSet<DnsEndPoint>(peers);

            Receive<NewPeers>(message => OnNewPeers(message));
        }

        private void OnNewPeers(NewPeers message)
        {
            foreach (var peer in message.Peers)
            {
                if (!peers.Contains(peer))
                {
                    peers.Add(peer);
                    Context.ActorOf(PeerActor.Props(torrent, peer));
                }
            }
        }
    }
}