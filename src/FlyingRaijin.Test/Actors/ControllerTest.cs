using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using FlyingRaijin.Controller.Actors;
using FlyingRaijin.Messages;
using System;
using Xunit;

namespace FlyingRaijin.Test.ControllerActors
{
    public class ControllerTest : TestKit
    {
        [Fact]
        public void NewTorrentRequest()
        {
            var message = new NewTorrentCommand("test");
            var subject = Sys.ActorOf<NewTorrentClientActor>();

            var probe = CreateTestProbe();
            subject.Tell(message);

            var result = ExpectMsg<NewTorrentCommand>(TimeSpan.FromSeconds(1));
            result.Should().NotBeNull();
            result.FilePath.Should().NotBeNull();
            result.FilePath.Should().BeEquivalentTo(message.FilePath);
        }
    }
}