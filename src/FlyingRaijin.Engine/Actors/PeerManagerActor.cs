using Akka.Actor;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections;

namespace FlyingRaijin.Engine.Actors
{
    public class PeerManagerActor : ReceiveActor
    {        
        private static readonly ReadOnlyMemory<byte> protocolIdentifierLength;
        private static readonly ReadOnlyMemory<byte> protocolIdentifier;        
        private static readonly ReadOnlyMemory<byte> reservedBytes = new byte[8];
        private static readonly MemorySegment<byte> handShakeHeader;
        private static readonly MemorySegment<byte> handShakeTrail;

        private readonly ReadOnlyMemory<byte> peerId;
        private readonly MetaData torrent;
        private readonly HashSet<DnsEndPoint> peers;
        private readonly ReadOnlySequence<byte> handShake;
        private readonly BitArray pieces;

        static PeerManagerActor()
        {
                  protocolIdentifier = Encoding.UTF8.GetBytes("BitTorrent protocol");
            protocolIdentifierLength = new byte[] { (byte)protocolIdentifier.Length };

            handShakeHeader = new MemorySegment<byte>(protocolIdentifierLength);

            handShakeTrail = handShakeHeader
                .Append(protocolIdentifier)
                .Append(reservedBytes);
        }

        public static Props Props(
            ReadOnlyMemory<byte> peerId,
            MetaData torrent)
        {
            return Akka.Actor.Props.Create(() => new PeerManagerActor(peerId, torrent));
        }

        public PeerManagerActor(
            ReadOnlyMemory<byte> peerId,
            MetaData torrent)
        {
            this.peerId = peerId;
            this.torrent = torrent;

            pieces = new BitArray((int)torrent.PieceLength, false);

            peers = new HashSet<DnsEndPoint>();

            var last = handShakeTrail
                .Append(torrent.InfoHash)
                .Append(peerId);

            handShake = new ReadOnlySequence<byte>(handShakeHeader, 0, last, last.Memory.Length);

            Receive<NewPeers>(message => OnNewPeers(message));
        }

        private void OnNewPeers(NewPeers message)
        {
            foreach (var peer in message.Peers.Skip(0))
            {
                if (!peers.Contains(peer))
                {
                    peers.Add(peer);

                    var peerActor = Context.ActorOf(PeerActor.Props(torrent.Pieces.Sha1Checksums.Count, peer, handShake));

                    peerActor.Tell(new ConnectCommand());

                    break;
                }
            }
        }
    }
}