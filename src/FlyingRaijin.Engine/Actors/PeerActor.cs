using Akka.Actor;
using Akka.IO;
using Akka.Streams.Dsl;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;
using IoTcp = Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Actors
{
    public class PeerActor : ReceiveActor
    {        
        private static readonly ReadOnlyMemory<byte> protocolIdentifierLength;
        private static readonly ReadOnlyMemory<byte> protocolIdentifier;        
        private static readonly ReadOnlyMemory<byte> reservedBytes = new byte[8];
        private static readonly MemorySegment<byte>  handShakeHeader;
        private static readonly MemorySegment<byte>  handShakeTrail;

        private readonly MetaData    torrent;
        private readonly DnsEndPoint endPoint;

        static PeerActor()
        {
                  protocolIdentifier = Encoding.UTF8.GetBytes("BitTorrent protocol");
            protocolIdentifierLength = new byte[] { (byte)protocolIdentifier.Length };

            handShakeHeader = new MemorySegment<byte>(protocolIdentifierLength);

            handShakeTrail = handShakeHeader
                .Append(protocolIdentifier)
                .Append(reservedBytes);
        }

        public static Props Props(MetaData torrent, DnsEndPoint endPoint)
        {
            return Akka.Actor.Props.Create(() => new PeerActor(torrent, endPoint));
        }

        public PeerActor(MetaData torrent, DnsEndPoint endPoint)
        {
             this.torrent = torrent;
            this.endPoint = endPoint;

            Receive<BeginHandShakeCommand>(command => OnBeginHandShakeCommand(command));
                Receive<IoTcp.CommandFailed>(message => OnCommandFailed(message));
             Receive<IoTcp.ConnectionClosed>(message => OnConnectionClosed(message));
                    Receive<IoTcp.Connected>(message => OnConnected(message));          
                     Receive<IoTcp.Received>(message => OnReceived(message));
        }

        private void OnBeginHandShakeCommand(BeginHandShakeCommand command)
        {
            Context.System.Tcp().Tell(new IoTcp.Connect(endPoint));           
        }

        private void OnCommandFailed(IoTcp.CommandFailed message)
        {
            System.Diagnostics.Debug.WriteLine($"command failed on {endPoint}");
        }

        private void OnConnectionClosed(IoTcp.ConnectionClosed message)
        {
            System.Diagnostics.Debug.WriteLine($"connection closed on {endPoint}");
        }

        private void OnConnected(IoTcp.Connected message)
        {
            Sender.Tell(new IoTcp.Register(Self, true, true));

            System.Diagnostics.Debug.WriteLine($"connected to {endPoint}");

            //var first = new MemorySegment<byte>(handShakeTrail.Memory);

            var last = handShakeTrail
                .Append(torrent.InfoHash)
                .Append(Encoding.UTF8.GetBytes("ABCDEFGHIJKLMNOPQRST"));

            var ros = new ReadOnlySequence<byte>(handShakeHeader, 0, last, last.Memory.Length);

            Sender.Tell(IoTcp.Write.Create(ByteString.FromBytes(ToArraySegment(ros))));
        }

        private void OnReceived(IoTcp.Received message)
        {
            System.Diagnostics.Debug.WriteLine($"received on {endPoint}");

            var flow = Flow.Create<ByteString>()
                .Via(Framing.Delimiter(ByteString.FromString("\n"),
                    256,
                    true))
                .Via(Flow.Create<ByteString>()
                    .Select(bytes =>
                    {
                        var message = Encoding.UTF8.GetString(bytes.ToArray());
                       //_logger.Info(message);
                        return message;
                    }))
                .Via(Flow.Create<string>()
                    .Select(s => "Hello World"))
                .Via(Flow.Create<string>()
                    .Select(s => s += "\n"))
                .Via(Flow.Create<string>()
                    .Select(ByteString.FromString));

            //var materializer = system.Materializer();
            //connection.Join(flow).Run(materializer);
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