using Akka.Actor;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Wire
{
    internal class MessageProcessorActor : ReceiveActor, IRecordMessage
    {
        internal MessageProcessorActor()
        {
            messageQueue = new Queue<IMessage>(50);

            Receive<MonitorMessageQueue>(_ => OnMonitorMessageQueue());
        }

        private readonly Queue<IMessage> messageQueue;

        public void Record(IMessage message)
        {
            messageQueue.Enqueue(message);
        }

        private void OnMonitorMessageQueue()
        {
            if (messageQueue.Count == 0)
            {
                return;
            }

            int maxMessageToProcess = 10;

            while (maxMessageToProcess != 0 && messageQueue.TryPeek(out IMessage message))
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

                maxMessageToProcess--;
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