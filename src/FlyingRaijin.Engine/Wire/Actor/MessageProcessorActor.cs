using Akka.Actor;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
    internal class MessageProcessorActorBuilder : ActorBuilderBase<ChannelReader<IMessage>, ChannelWriter<PieceMessage>>
    {
        internal override IActorRef Build(IUntypedActorContext context)
        {
            return context.ActorOf(Props.Create(() => new MessageProcessorActor(Value1, Value2)));
        }
    }

    internal class MessageProcessorActor : ReceiveActor
    {
        internal MessageProcessorActor(
            ChannelReader<IMessage> channelReaderMessage,
            ChannelWriter<PieceMessage> channelWriterPiece)
        {
            this.channelReaderMessage = channelReaderMessage;
            this.channelWriterPiece = channelWriterPiece;

            Receive<MonitorMessageQueue>(_ => OnMonitorChannelReaderMessage());            
        }

        private readonly ChannelReader<IMessage> channelReaderMessage;
        private readonly ChannelWriter<PieceMessage> channelWriterPiece;

        private void OnMonitorChannelReaderMessage()
        {
            int maxMessageToProcess = 10;

            while (maxMessageToProcess != 0 && channelReaderMessage.TryRead(out IMessage message))
            {
                switch (message.MessageId)
                {
                    case MessageId.Choke:
                        ProcessChoke(message);
                        break;
                    case MessageId.Unchoke:
                        ProcessUnchoke(message);
                        break;
                    case MessageId.Interested:
                        ProcessInterested(message);
                        break;
                    case MessageId.NotInterested:
                        ProcessNotInterested(message);
                        break;
                    case MessageId.Have:
                        ProcessHave(message);
                        break;
                    case MessageId.BitField:
                        ProcessBitField(message);
                        break;
                    case MessageId.Request:
                        ProcessRequest(message);
                        break;
                    case MessageId.Piece:
                        ProcessPiece(message);
                        break;
                    case MessageId.Cancel:
                        ProcessCancel(message);
                        break;
                    case MessageId.Port:
                        ProcessPort(message);
                        break;
                    case MessageId.UnKnown:
                        ProcessUnknown();
                        break;
                    default:
                        break;
                }

                --maxMessageToProcess;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessChoke(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessUnchoke(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessInterested(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessNotInterested(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessHave(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessBitField(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessRequest(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessPiece(IMessage message)
        {
            channelWriterPiece.TryWrite((PieceMessage)message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessCancel(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessPort(IMessage message)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessUnknown()
        {

        }
    }
}