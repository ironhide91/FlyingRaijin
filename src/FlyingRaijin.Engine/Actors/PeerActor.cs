using Akka.Actor;
using Akka.IO;
using FlyingRaijin.Engine.Messages;
using FlyingRaijin.Engine.Wire;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using static Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Actors
{
    public partial class PeerActor : ReceiveActor
    {
        public static Props Props(long pieceLength, DnsEndPoint endPoint, ReadOnlySequence<byte> handshake)
        {
            return Akka.Actor.Props.Create(() => new PeerActor(pieceLength, endPoint, handshake));
        }
        
        private readonly ReadOnlySequence<byte> handshake;
        private readonly DnsEndPoint endPoint;
        private readonly long pieceLength;
        private readonly Pipe pipe;

        private IActorRef remote;
        private RemotePeerState remoteState;       

        public PeerActor(long pieceLength, DnsEndPoint endPoint, ReadOnlySequence<byte> handshake)
        {
            this.pieceLength = pieceLength;
            this.endPoint = endPoint;
            this.handshake = handshake;

            pipe = new Pipe();

            remoteState = RemotePeerState.Disconnected;

            Receive<ConnectCommand>(command => OnConnectCommand(command));
            Receive<CommandFailed>(message => OnCommandFailed(message));
            Receive<ConnectionClosed>(message => OnConnectionClosed(message));
            Receive<Connected>(message => OnConnected(message));          
            Receive<BeginHandshake>(message => OnBeginHandshake(message));
            Receive<Received>(message => OnReceived(message));
            Receive<PipeWritten>(message => OnPipeWritten(message));
        }

        private void OnConnectCommand(ConnectCommand command)
        {
            Context.System.Tcp().Tell(new Connect(endPoint));

            remoteState = RemotePeerState.Connecting;
        }

        private void OnCommandFailed(CommandFailed message)
        {
            System.Diagnostics.Debug.WriteLine($"command failed on {endPoint}");

            remoteState = RemotePeerState.Failed;
        }

        private void OnConnectionClosed(ConnectionClosed message)
        {
            System.Diagnostics.Debug.WriteLine($"connection closed on {endPoint}");

            remoteState = RemotePeerState.Closed;
        }

        private void OnConnected(Connected message)
        {
            remoteState = RemotePeerState.Connected;

            Sender.Tell(new Register(Self));

            remote = Sender;

            System.Diagnostics.Debug.WriteLine($"connected to {message.RemoteAddress}");

            Self.Tell(new BeginHandshake());
        }

        private void OnBeginHandshake(BeginHandshake _)
        {
            remote.Tell(
                Write.Create(
                    ByteString.FromBytes(
                        ToArraySegment(handshake))));

            remoteState = RemotePeerState.HandshakeInProgress;
        }

        private async void OnReceived(Received message)
        {
            System.Diagnostics.Debug.WriteLine($"received on {endPoint}");

            await ProtocolHelper.Push(pipe.Writer, message.Data.ToArray());

            Self.Tell(new PipeWritten());
        }

        private void OnPipeWritten(PipeWritten _)
        {
            pipe.Reader.TryRead(out ReadResult result);

            OnPipeWritten(pipe.Reader, result.Buffer);
        }

        private void OnPipeWritten(PipeReader reader, ReadOnlySequence<byte> buffer)
        {
            var hsReader = new SequenceReader<byte>(handshake);

            var seqReader = new SequenceReader<byte>(buffer);

            if (remoteState == RemotePeerState.HandshakeInProgress)
            {
                if (ProtocolHelper.IsValidHandshakeResponse(ref hsReader, ref seqReader))
                {                    
                    //reader.AdvanceTo(seqReader.Position);
                    remoteState = RemotePeerState.HandshakeSuccessfull;
                    //return;
                }
            }

            if (remoteState == RemotePeerState.HandshakeSuccessfull)
            {
                int length;

                do
                {
                    length = ProtocolHelper.TryReadMessageLength(ref seqReader);                    
                }
                while (length == 0);

                var messageType = ProtocolHelper.TryReadMessageType(ref seqReader);                

                if (messageType == MessageType.UnKnown)
                {
                    remote.Tell(Close.Instance);
                    return;
                }

                if ((messageType == MessageType.BitField) && (((length-1) * 8) != pieceLength))
                {
                    remote.Tell(Close.Instance);
                    return;
                }

                if (true)
                {
                    
                }

                reader.AdvanceTo(seqReader.Position);

                return;
            }
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