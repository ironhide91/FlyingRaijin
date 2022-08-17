using Akka.Actor;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
    internal class WireProtocolActor : ReceiveActor, IWithTimers
    {
        internal WireProtocolActor(
            ReadOnlyMemory<byte> handshake,
            PipeReader pipeReader,
            IRequestPieceBlock pieceBlock,
            ChannelWriter<IMessage> channelWriterMessage)
        {
            this.handshake = handshake;
            this.pipeReader = pipeReader;
            this.pieceBlock = pieceBlock;
            this.channelWriterMessage = channelWriterMessage;
            handshakeStatus = HandshakeStatus.Uninitiated;

            Receive<HandshakeInitiated>(_ => OnHandshakeInitiated());
            Receive<MonitorPipeForData>(_ => OnMonitorPipeForData());
        }

        private readonly ReadOnlyMemory<byte> handshake;
        private readonly PipeReader pipeReader;
        IRequestPieceBlock pieceBlock;
        private readonly ChannelWriter<IMessage> channelWriterMessage;

        private HandshakeStatus handshakeStatus;
        private MessageId currentMessageId;
        private int pendingMessageLength;

        public ITimerScheduler Timers { get; set; }

        protected override void PreStart()
        {
            Timers.StartPeriodicTimer(
                nameof(MonitorPipeForData),
                MonitorPipeForData.Instance,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(5));
        }

        private void OnHandshakeInitiated()
        {
            handshakeStatus = HandshakeStatus.Initiated;
        }

        private void OnMonitorPipeForData()
        {
            if (handshakeStatus == HandshakeStatus.Uninitiated)
                return;

            if (handshakeStatus == HandshakeStatus.Initiated)
            {
                var response = pipeReader.IsValidHandshakeResponse(handshake);

                handshakeStatus = response switch
                {
                    HandshakeResponse.Succes => HandshakeStatus.Success,
                    HandshakeResponse.Pending => HandshakeStatus.Initiated,
                    HandshakeResponse.Failed => HandshakeStatus.Failed,
                    _ => HandshakeStatus.Unknown,
                };

                return;
            }

            if (handshakeStatus == HandshakeStatus.Failed)
            {
                return;
            }

            ReadPipe();
        }

        private void ReadPipe()
        {
            if (!pipeReader.TryRead(out ReadResult result))
            {
                return;
            }

            var seqReader = new SequenceReader<byte>(result.Buffer);

            while (seqReader.HasData())
            {
                if (pendingMessageLength > 0 && currentMessageId.IsUnknownMessage())
                {
                    ParseIncompleteUnkownMessage(ref seqReader, pendingMessageLength);
                    pendingMessageLength = 0;
                    continue;
                }

                if (pendingMessageLength > 0 && currentMessageId.IsDataMessage())
                {
                    ParseIncompleteDataMessage(ref seqReader, currentMessageId, pendingMessageLength);
                    pendingMessageLength = 0;
                    continue;
                }

                // message header length (4 bytes) + message id (1 byte)
                if (seqReader.IsSequenceIncomplete(4))
                {
                    return;
                }

                var messageLength = pipeReader.ReadMessageLength();

                if (messageLength == 0)
                {
                    // keep alive
                    continue;
                }

                var messageId = pipeReader.ReadMessageId();

                messageLength--;

                if (messageId.IsUnknownMessage() && seqReader.IsSequenceIncomplete(messageLength))
                {
                    currentMessageId = messageId;
                    pendingMessageLength = messageLength;
                    return;
                }

                if (messageId.IsControlMessage())
                {
                    if (messageLength > 1)
                    {
                        // control message should have length 1
                        // skip
                        continue;
                    }

                    channelWriterMessage.TryWrite(messageId.CreateControlMessage());
                    continue;
                }

                if (messageId.IsDataMessage() && seqReader.IsSequenceIncomplete(messageLength))
                {
                    currentMessageId = messageId;
                    pendingMessageLength = messageLength;
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParseIncompleteUnkownMessage(ref SequenceReader<byte> seqReader, long pendingMessageLength)
        {
            seqReader.Advance(pendingMessageLength);
            pipeReader.AdvanceTo(seqReader.Position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParseIncompleteDataMessage(ref SequenceReader<byte> seqReader, MessageId id, long pendingMessageLength)
        {
            switch (id)
            {
                case MessageId.Have:
                    {
                        var result = seqReader.TryParseHaveMessage();
                        if (result.Item1)
                        {
                            // error
                        }
                        channelWriterMessage.TryWrite(result.Item2);
                    }
                    break;
                case MessageId.BitField:
                    {
                        var result = seqReader.TryParseBitFieldMessage(pendingMessageLength);
                        if (result.Item1)
                        {
                            // error
                        }
                        channelWriterMessage.TryWrite(result.Item2);
                    }
                    break;
                case MessageId.Request:
                    {
                        var result = seqReader.TryParseRequestMessage();
                        if (result.Item1)
                        {
                            // error
                        }
                        channelWriterMessage.TryWrite(result.Item2);
                    }
                    break;
                case MessageId.Piece:
                    {
                        var result = seqReader.TryParsePieceMessage(pieceBlock, pendingMessageLength);
                        if (result.Item1)
                        {
                            // error
                        }
                        channelWriterMessage.TryWrite(result.Item2);
                    }
                    break;
                case MessageId.Port:
                    {
                        var result = seqReader.TryParsePortMessage();
                        if (result.Item1)
                        {
                            // error
                        }
                        channelWriterMessage.TryWrite(result.Item2);
                    }
                    break;
                default:
                    break;
            }

            pipeReader.AdvanceTo(seqReader.Position);
        }       
    }

    internal class WireProtocolActorBuilder :
        ActorBuilderBase<
            ReadOnlyMemory<byte>,
            PipeReader,
            IReadOnlyDictionary<int, PieceBlock>,
            ChannelWriter<IMessage>>
    {
        private readonly Props ctor;

        internal WireProtocolActorBuilder()
        {
            ctor = Props.Create(() => new WireProtocolActor(Value1, Value2, Value3, Value4));
        }
    }
}