using Akka.Actor;
using Akka.IO;
using FlyingRaijin.Engine.Messages.Peer;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using static Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Wire
{
    internal class TcpActorBuilder : ActorBuilderBase<DnsEndPoint, PipeWriter>
    {
        internal TcpActorBuilder() : base()
        {
            ctor = Props.Create(() => new TcpActor(Value1, Value2));
        }

        //internal override Props Build()
        //{
        //    return ctor;
        //}

        //internal override IActorRef Build(IUntypedActorContext context)
        //{
        //    return context.ActorOf(ctor);
        //}

        //internal override IActorRef Build(ActorSystem system)
        //{
        //    return system.ActorOf(ctor);
        //}
    }

    internal class TcpActor : ReceiveActor
    {
        public TcpActor(DnsEndPoint endPoint, PipeWriter pipeWriter)
        {
            this.endPoint = endPoint;
            this.pipeWriter = pipeWriter;

            Become(InitialBehaviour);
        }

        private readonly DnsEndPoint endPoint;
        private readonly PipeWriter pipeWriter;
        private IActorRef remotePeer;

        #region Actor Behaviours
        private void InitialBehaviour()
        {
            Receive<PeerConnectMessage>(_ => OnConnectCommand());
        }

        private void ConnectingBehaviour()
        {
            Receive<CommandFailed>(message => OnCommandFailed(message));
            Receive<Connected>(message => OnConnected(message));
        }

        private void ConnectedBehaviour()
        {
            Receive<Received>(message => OnReceived(message));
            Receive<CommandFailed>(message => OnCommandFailed(message));
            Receive<ConnectionClosed>(message => OnConnectionClosed(message));
            Receive<PeerWriteMessage>(message => OnWrite(message));
        }

        private void ConnectionClosedBehaviour()
        {
            Self.Tell(PoisonPill.Instance);
        }

        private void ErrorBehaviour()
        {
            Self.Tell(PoisonPill.Instance);
        }
        #endregion

        #region Message Handlers
        private void OnConnectCommand()
        {
            Become(ConnectingBehaviour);            
            Context.System.Tcp().Tell(new Connect(endPoint));
            Sender.Tell(PeerConnectingMessage.Instance);
        }

        private void OnCommandFailed(CommandFailed message)
        {
            Become(ErrorBehaviour);

            System.Diagnostics.Debug.WriteLine($"command failed on {endPoint}");
        }

        private void OnConnectionClosed(ConnectionClosed message)
        {
            Become(ConnectionClosedBehaviour);
            System.Diagnostics.Debug.WriteLine($"connection closed on {endPoint}");
        }

        private void OnConnected(Connected message)
        {
            Become(ConnectedBehaviour);
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
        #endregion
    }
}