using Akka.Actor;
using Akka.IO;
using FlyingRaijin.Engine.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Threading.Tasks;
using static Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Actors
{
    public class PeerActor : ReceiveActor
    {
        public static Props Props(DnsEndPoint endPoint, ReadOnlySequence<byte> handshake)
        {
            return Akka.Actor.Props.Create(() => new PeerActor(endPoint, handshake));
        }
        
        private readonly ReadOnlySequence<byte> handshake;
        private readonly DnsEndPoint endPoint;        
        private readonly Pipe pipe;

        private IActorRef remote;
        private RemotePeerState remoteState;       

        public PeerActor(DnsEndPoint endPoint, ReadOnlySequence<byte> handshake)
        {
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

            System.Diagnostics.Debug.WriteLine($"connected to {endPoint}");

            Self.Tell(new BeginHandshake());
        }

        private void OnBeginHandshake(BeginHandshake message)
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

            await PeerWireProtocolHelper.Push(pipe.Writer, message.Data.ToArray());

            Self.Tell(new PipeWritten());
        }

        private async Task OnPipeWritten(PipeWritten message)
        {
            var result = await pipe.Reader.ReadAsync();

            if (remoteState == RemotePeerState.HandshakeInProgress)
            {
                if (PeerWireProtocolHelper.IsValidHandshakeResponse(pipe.Reader, handshake, result.Buffer))
                {
                    remoteState = RemotePeerState.HandshakeSuccessfull;
                    return;
                }
            }

            if (remoteState == RemotePeerState.HandshakeSuccessfull)
            {
                PeerWireProtocolHelper.TryReadMessage(pipe.Reader, result.Buffer);
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

        struct PipeWritten
        {

        }

        enum RemotePeerState
        {           
            Failed,
            Closed,
            Disconnected,
            Connecting,
            Connected,
            HandshakeInProgress,
            HandshakeSuccessfull,
            HandshakeUnsuccessfull,
            Payload
        }
    }
}