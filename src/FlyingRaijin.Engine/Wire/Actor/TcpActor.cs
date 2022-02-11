using Akka.Actor;
using Akka.IO;
using FlyingRaijin.Engine.Messages.Peer;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using static Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Wire
{
    internal class TcpActor : ReceiveActor
    {
        public TcpActor(DnsEndPoint endPoint, PipeWriter pipeWriter)
        {
            this.endPoint = endPoint;
            this.pipeWriter = pipeWriter;

            Receive<PeerConnectMessage>(message => OnConnectCommand(message));
            Receive<CommandFailed>(message => OnCommandFailed(message));
            Receive<ConnectionClosed>(message => OnConnectionClosed(message));
            Receive<Connected>(message => OnConnected(message));
            Receive<Received>(message => OnReceived(message));
            Receive<PeerWriteMessage>(message => OnWrite(message));
        }

        private readonly DnsEndPoint endPoint;
        private readonly PipeWriter pipeWriter;
        private IActorRef remotePeer;

        private void OnConnectCommand(PeerConnectMessage command)
        {
            Context.System.Tcp().Tell(new Connect(endPoint));
        }

        private void OnCommandFailed(CommandFailed message)
        {
            System.Diagnostics.Debug.WriteLine($"command failed on {endPoint}");
        }

        private void OnConnectionClosed(ConnectionClosed message)
        {
            System.Diagnostics.Debug.WriteLine($"connection closed on {endPoint}");
        }

        private void OnConnected(Connected message)
        {
            Sender.Tell(new Register(Self));
            remotePeer = Sender;
            Context.Parent.Tell(PeerConnectedMessage.Instance);
            System.Diagnostics.Debug.WriteLine($"connected to {message.RemoteAddress}");
        }

        private void OnReceived(Received message)
        {
            System.Diagnostics.Debug.WriteLine($"received on {endPoint}");
            pipeWriter.Write(message.Data.ToArray());
            pipeWriter.Advance(message.Data.Count);
            pipeWriter.FlushAsync();

            Context.Parent.Tell(PeerPipeWritten.Instance);
        }

        private void OnWrite(PeerWriteMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"writing to {endPoint}");
            remotePeer.Tell(Write.Create(ByteString.FromBytes(message.Data.Span.ToArray())));
            System.Diagnostics.Debug.WriteLine($"written to {endPoint}");
        }
    }
}