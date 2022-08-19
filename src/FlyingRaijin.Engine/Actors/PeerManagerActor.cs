using FlyingRaijin.Engine.Messages;
using FlyingRaijin.Engine.Messages.Peer;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Wire;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FlyingRaijin.Engine.Actors
{
    internal class PeerManagerActor : ReceiveActor
    {
        private static readonly ReadOnlyMemory<byte> handShakeHeader;

        private readonly ReadOnlyMemory<byte> handshake;
        private readonly ReadOnlyMemory<byte> myPeerId;
        private readonly MetaData torrent;
        private readonly HashSet<DnsEndPoint> peers;
        private readonly BitArray pieces;

        static PeerManagerActor()
        {
            var protocolIdentifier = Encoding.UTF8.GetBytes("BitTorrent protocol");            
            var header = new byte[20];

            header[0] = (byte)protocolIdentifier.Length;
            protocolIdentifier.CopyTo(header, 1);

            handShakeHeader = new ReadOnlyMemory<byte>(header);
        }

        internal static Props Props(ReadOnlyMemory<byte> myPeerId, MetaData torrent)
        {
            return Akka.Actor.Props.Create(() => new PeerManagerActor(myPeerId, torrent));
        }

        internal PeerManagerActor(ReadOnlyMemory<byte> myPeerId, MetaData torrent)
        {
            this.myPeerId = myPeerId;
            this.torrent = torrent;

            pieces = new BitArray((int)torrent.PieceLength, false);
            peers = new HashSet<DnsEndPoint>();

            Memory<byte> handshakeTemp = new byte[68];
            handShakeHeader.CopyTo(handshakeTemp);
            //torrent.InfoHash.CopyTo(handshakeTemp);
            myPeerId.CopyTo(handshakeTemp);
            handshake = handshakeTemp;

            Receive<NewPeers>(message => OnNewPeers(message));
        }

        private void OnNewPeers(NewPeers message)
        {
            foreach (var peer in message.Peers.Skip(0))
            {
                if (!peers.Contains(peer))
                {
                    peers.Add(peer);
                    
                    var peerActorBuilder = new PeerActorBuilder();
                    peerActorBuilder.With(torrent);
                    peerActorBuilder.With(peer);
                    peerActorBuilder.With(handshake);

                    var peerActor = peerActorBuilder.Build(Context);
                    peerActor.Tell(PeerConnectMessage.Instance);
                    
                    break;
                }
            }
        }
    }
}