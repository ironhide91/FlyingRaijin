using Akka.TestKit.Xunit2;
using FluentAssertions;
using FlyingRaijin.Engine.Messages.Peer;
using FlyingRaijin.Engine.Wire;
using System;
using System.IO.Pipelines;
using System.Net;
using Xunit;

namespace FlyingRaijin.Test.Wire
{
    public class TcpActorTest : TestKit
    {
        [Fact]
        public void InitialBehaviourExpectNoMsg()
        {
            var probe = CreateTestProbe();

            var address = IPAddress.Any;
            var pipe = new Pipe();

            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(new DnsEndPoint(address.ToString(), 6767));
            tcpActorBuilder.With(pipe.Writer);

            var tcpActor = probe.ChildActorOf(tcpActorBuilder.Build());
            
            tcpActor.Tell(MockTcpMessage.Failed, probe);
            ExpectNoMsg();

            tcpActor.Tell(MockTcpMessage.Connected, probe);
            ExpectNoMsg();

            tcpActor.Tell(MockTcpMessage.Closed, probe);
            ExpectNoMsg();

            tcpActor.Tell(MockTcpMessage.Received, probe);
            ExpectNoMsg();

            tcpActor.Tell(MockTcpMessage.Received, probe);
            ExpectNoMsg();
        }

        [Fact]
        public void InitialBehaviourExpectMsg()
        {
            var probe = CreateTestProbe();

            var address = IPAddress.Any;
            var pipe = new Pipe();

            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(new DnsEndPoint(address.ToString(), 6767));
            tcpActorBuilder.With(pipe.Writer);
            
            var tcpActor = probe.ChildActorOf(tcpActorBuilder.Build());

            tcpActor.Tell(PeerConnectMessage.Instance, probe);

            var result = probe.ExpectMsg<PeerConnectingMessage>(TimeSpan.FromSeconds(1));
            result.Should().NotBeNull();
        }

        [Fact]
        public void ConnectingBehaviourExpectNoMsg()
        {
            var address = IPAddress.Any;
            var pipe = new Pipe();

            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(new DnsEndPoint(address.ToString(), 6767));
            tcpActorBuilder.With(pipe.Writer);

            var tcpActor = tcpActorBuilder.Build(Sys);
            var probe = CreateTestProbe();
            probe.Watch(tcpActor);

            tcpActor.Tell(PeerConnectMessage.Instance, probe);
            var result = probe.ExpectMsg<PeerConnectingMessage>(TimeSpan.FromSeconds(1));
            result.Should().NotBeNull();

            tcpActor.Tell(PeerConnectMessage.Instance, probe);
            ExpectNoMsg();

            tcpActor.Tell(MockTcpMessage.Received, probe);
            ExpectNoMsg();

            tcpActor.Tell(MockTcpMessage.Closed, probe);
            ExpectNoMsg();

            tcpActor.Tell(PeerWriteMessage.Empty, probe);
            ExpectNoMsg();
        }

        [Fact]
        public void ConnectingBehaviourFailed()
        {
            var probe = CreateTestProbe(); 
            
            var address = IPAddress.Any;
            var pipe = new Pipe();

            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(new DnsEndPoint(address.ToString(), 6767));
            tcpActorBuilder.With(pipe.Writer);

            var tcpActor = probe.ChildActorOf(tcpActorBuilder.Build());

            probe.Watch(tcpActor);

            tcpActor.Tell(PeerConnectMessage.Instance, probe);
            var result = probe.ExpectMsg<PeerConnectingMessage>(TimeSpan.FromSeconds(1));
            result.Should().NotBeNull();

            tcpActor.Tell(MockTcpMessage.Failed, probe);
            probe.ExpectTerminated(tcpActor);
        }

        [Fact]
        public void ConnectingBehaviourConnected()
        {
            var probe = CreateTestProbe();

            var address = IPAddress.Any;
            var pipe = new Pipe();

            var tcpActorBuilder = new TcpActorBuilder();
            tcpActorBuilder.With(new DnsEndPoint(address.ToString(), 6767));
            tcpActorBuilder.With(pipe.Writer);

            var tcpActor = probe.ChildActorOf(tcpActorBuilder.Build());
            
            probe.Watch(tcpActor);

            {
                tcpActor.Tell(PeerConnectMessage.Instance, probe);
                var result = probe.ExpectMsg<PeerConnectingMessage>(TimeSpan.FromSeconds(1));
                result.Should().NotBeNull();
            }

            {
                tcpActor.Tell(MockTcpMessage.Connected, probe);
                var result = probe.ExpectMsg<PeerConnectedMessage>(TimeSpan.FromSeconds(1));
                result.Should().NotBeNull();
            }
        }
    }
}