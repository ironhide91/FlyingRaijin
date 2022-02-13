using Akka.Actor;
using FlyingRaijin.Engine.Messages.Peer;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;

namespace FlyingRaijin.Engine.Wire
{
    internal partial class PeerActor : ReceiveActor
    {
        internal PeerActor(MetaData torrent, DnsEndPoint endPoint, ReadOnlyMemory<byte> handshake)
        {
            this.torrent = torrent;
            this.endPoint = endPoint;
            this.handshake = handshake;
            pipe = new Pipe();

            Receive<PeerConnectMessage>(_ => OnConnectCommand());
            Receive<PeerConnectedMessage>(_ => OnConnected());
        }

        private ReadOnlyMemory<byte> handshake;
        private readonly MetaData torrent;
        private readonly DnsEndPoint endPoint;
        private readonly Pipe pipe;

        private IActorRef tcpActor;
        private IActorRef wireProtocolActor;
        private IActorRef messageProcessorActor;
        private IActorRef pieceWriterActor;

        private void OnConnectCommand()
        {
            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(endPoint);
            tcpActorBuilder.With(pipe.Writer);
            tcpActor = Context.ActorOf(tcpActorBuilder.Build());
            tcpActor.Tell(PeerConnectMessage.Instance);            

            var wireProtocolActorBuilder = new WireProtocolActorBuilder();
            wireProtocolActorBuilder.With(handshake);
            wireProtocolActorBuilder.With(pipe.Reader);
            wireProtocolActorBuilder.With((IRecordMessage)messageProcessorActor);
            wireProtocolActor = Context.ActorOf(wireProtocolActorBuilder.Build());

            messageProcessorActor = Context.ActorOf(Props.Create(() => new MessageProcessorActor()));

            var pieceWriterActorBuilder = new PieceWriterActorBuilder();
            pieceWriterActorBuilder.With(handshake);
            pieceWriterActorBuilder.With(pipe.Reader);
            pieceWriterActorBuilder.With((IRecordMessage)messageProcessorActor);
            pieceWriterActorBuilder = Context.ActorOf(wireProtocolActorBuilder.Build());
        }

        private void OnConnected()
        {
            // send handshake
            tcpActor.Tell(PeerWriteMessage.Create(handshake));

            // notify handshake in progress
            wireProtocolActor.Tell(HandshakeInitiated.Instance);
        }

        private IEnumerable<ArraySegment<byte>> ToArraySegment(ReadOnlySequence<byte> ros)
        {
            List<ArraySegment<byte>> arraySegment = new List<ArraySegment<byte>>();

            var temp = ros.GetEnumerator();

            while (temp.MoveNext())
            {
                arraySegment.Add(temp.Current.Span.ToArray());
            }

            return arraySegment;
        }
    }

    
}