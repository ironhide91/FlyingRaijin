using Akka.Actor;
using Akka.IO;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Net;
using System.Text;

namespace FlyingRaijin.Engine.Actors
{
    internal class PeerActor<T> : ReceiveActor where  T : TorrentBase<InfoDictionaryBase>
    {
        private readonly T torrent;

        private static readonly MemorySegment<byte>  handShakeHeader;
        private static readonly ReadOnlyMemory<byte> protocolIdentifierLength;
        private static readonly ReadOnlyMemory<byte> protocolIdentifier;
        private static          ReadOnlyMemory<byte> reservedBytes = new byte[8];

        static PeerActor()
        {
                  protocolIdentifier = Encoding.UTF8.GetBytes("BitTorrent protocol");
            protocolIdentifierLength = BitConverter.GetBytes(protocolIdentifier.Length);

            handShakeHeader = new MemorySegment<byte>(protocolIdentifierLength);
            handShakeHeader.Append(protocolIdentifier);
            handShakeHeader.Append(reservedBytes);
        }

        public PeerActor(T peerOf, DnsEndPoint endPoint)
        {
            torrent = peerOf;

            Context.System.Tcp().Tell(new Tcp.Connect(endPoint));

            Receive<Tcp.Connected>(x => OnConnected(x));
            Receive<Tcp.Received>(x => OnReceived(x));
        }

        private void OnConnected(Tcp.Connected message)
        {
            Sender.Tell(new Tcp.Register(Self, true, true));
            Tcp.Write.Create(null);

            var handshake = new MemorySegment<byte>(handShakeHeader.Memory);
            handshake.Append(torrent.InfoHash);
            handshake.Append(torrent.InfoHash);
        }

        private void OnReceived(Tcp.Received message)
        {

        }
    }
}