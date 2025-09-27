using Akka.Actor;
using Akka.IO;
using FlyingRaijin.Engine.Messages;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using static Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Wire
{
    internal class TcpActor : ReceiveActor
    {
        private readonly DnsEndPoint endPoint;        
        private readonly Pipe pipe;

        private IActorRef remotePeer;

        public TcpActor(DnsEndPoint endPoint, Pipe pipe)
        {
            this.endPoint = endPoint;
            this.pipe = pipe;

            Receive<ConnectCommand>(command => OnConnectCommand(command));
            Receive<CommandFailed>(message => OnCommandFailed(message));
            Receive<ConnectionClosed>(message => OnConnectionClosed(message));
            Receive<Connected>(message => OnConnected(message));
            Receive<Received>(message => OnReceived(message));
        }

        private void OnConnectCommand(ConnectCommand command)
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
            System.Diagnostics.Debug.WriteLine($"connected to {message.RemoteAddress}");
        }

        private async void OnReceived(Received message)
        {
            System.Diagnostics.Debug.WriteLine($"received on {endPoint}");
            pipe.Writer.Write(message.Data.ToArray());
            pipe.Writer.Advance(message.Data.Count);
            _ = await pipe.Writer.FlushAsync();
        }
    }
}