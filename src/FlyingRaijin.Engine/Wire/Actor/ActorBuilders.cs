using Akka.Actor;
using FlyingRaijin.Engine.Torrent;
using System;
using System.IO.Pipelines;
using System.Net;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
    internal class PeerActorBuilder : ActorBuilderBase<long, DnsEndPoint, ReadOnlyMemory<byte>>
    {
        internal override Props Build()
        {
            return Props.Create(() => new PeerActor(Value1, Value2, Value3));
        }
    }

    internal class TcpActorBuilder : ActorBuilderBase<DnsEndPoint, PipeWriter>
    {
        internal override Props Build()
        {
            return Props.Create(() => new TcpActor(Value1, Value2));
        }
    }   

    internal class WireProtocolActorBuilder : ActorBuilderBase<ReadOnlyMemory<byte>, PipeReader, IRecordMessage>
    {
        internal override Props Build()
        {
            return Props.Create(() => new WireProtocolActor(Value1, Value2, Value3));
        }
    }

    internal class WireProtocolActorBuilder : ActorBuilderBase<ReadOnlyMemory<byte>, PipeReader, IRecordMessage>
    {
        internal override Props Build()
        {
            return Props.Create(() => new MessageProcessorActor(Value1, Value2, Value3));
        }
    }

    internal class PieceWriterActorBuilder : ActorBuilderBase<MetaData, ChannelReader<PieceMessage>, ChannelWriter<CompletePiece>>
    {
        internal override Props Build()
        {
            return Props.Create(() => new PieceWriterActor(Value1, Value2, Value3));
        }
    }
}