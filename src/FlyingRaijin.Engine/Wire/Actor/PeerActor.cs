using Akka.Actor;
using FlyingRaijin.Engine.Messages.Peer;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
    internal class PeerActorBuilder : ActorBuilderBase<MetaData, DnsEndPoint, ReadOnlyMemory<byte>, ChannelWriter<CompletePiece>>
    {
        private readonly Props ctor;

        internal PeerActorBuilder()
        {
            ctor = Props.Create(() => new PeerActor(Value1, Value2, Value3, Value4));
        }

        //internal override Props Build()
        //{
        //    return ctor;
        //}

        //internal override IActorRef Build(IUntypedActorContext context)
        //{
        //    return context.ActorOf(Props.Create(() => new PeerActor(Value1, Value2, Value3, Value4)));
        //}

        //internal override IActorRef Build(ActorSystem system)
        //{
        //    return system.ActorOf(Props.Create(() => new PeerActor(Value1, Value2, Value3, Value4)));
        //}
    }

    internal partial class PeerActor : ReceiveActor
    {
        internal PeerActor(
            MetaData torrent,
            DnsEndPoint endPoint,
            ReadOnlyMemory<byte> handshake,
            ChannelWriter<CompletePiece> channelWriterCompletePiece)
        {
            this.torrent = torrent;
            this.endPoint = endPoint;
            this.handshake = handshake;
            this.channelWriterCompletePiece = channelWriterCompletePiece;

            pipe = new Pipe();

            channelMessage = Channel.CreateUnbounded<IMessage>(
                new UnboundedChannelOptions()
                {
                    SingleReader = true,
                    SingleWriter = true
                });

            channelPieceMessage = Channel.CreateUnbounded<PieceMessage>(
                new UnboundedChannelOptions()
                {
                    SingleReader = true,
                    SingleWriter = true
                });

            Receive<PeerConnectMessage>(_ => OnConnectCommand());
            Receive<PeerConnectedMessage>(_ => OnConnected());
        }

        private ReadOnlyMemory<byte> handshake;
        private readonly MetaData torrent;
        private readonly DnsEndPoint endPoint;
        private readonly Pipe pipe;
        private readonly Channel<IMessage> channelMessage;
        private readonly Channel<PieceMessage> channelPieceMessage;
        private readonly ChannelWriter<CompletePiece> channelWriterCompletePiece;

        private IActorRef tcpActor;
        private IActorRef wireProtocolActor;
        private IActorRef messageProcessorActor;
        private IActorRef pieceWriterActor;

        private void OnConnectCommand()
        {
            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(endPoint);
            tcpActorBuilder.With(pipe.Writer);
            tcpActor = tcpActorBuilder.Build(Context);
            tcpActor.Tell(PeerConnectMessage.Instance);            

            var wireProtocolActorBuilder = new WireProtocolActorBuilder();
            wireProtocolActorBuilder.With(handshake);
            wireProtocolActorBuilder.With(pipe.Reader);
            wireProtocolActorBuilder.With(channelMessage.Writer);
            wireProtocolActor = wireProtocolActorBuilder.Build(Context);

            var messageProcessorActorBuilder = new MessageProcessorActorBuilder();
            messageProcessorActorBuilder.With(channelMessage.Reader);
            messageProcessorActorBuilder.With(channelPieceMessage.Writer);
            messageProcessorActor = messageProcessorActorBuilder.Build(Context);

            var pieceWriterActorBuilder = new PieceWriterActorBuilder();
            pieceWriterActorBuilder.With(torrent);
            pieceWriterActorBuilder.With(channelPieceMessage.Reader);
            pieceWriterActorBuilder.With(channelWriterCompletePiece);
            pieceWriterActor = pieceWriterActorBuilder.Build(Context);
        }

        private void OnConnected()
        {
            // send handshake
            tcpActor.Tell(PeerWriteMessage.Create(handshake));

            // notify handshake in progress
            wireProtocolActor.Tell(HandshakeInitiated.Instance);
        }
    }
}