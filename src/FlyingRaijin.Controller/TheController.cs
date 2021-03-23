using Akka.Actor;
using FlyingRaijin.Controller.Actors;
using FlyingRaijin.Messages;
using System;

namespace FlyingRaijin.Controller
{
    public class TheController : IDisposable
    {
        private TheController()
        {
            clientActorSystem = ActorSystem.Create("Client");

            newTorrentClientActorRef = clientActorSystem.ActorOf<NewTorrentClientActor>();
        }

        public static TheController Instance { get { return lazy.Value; } }

        private static Lazy<TheController> lazy = new Lazy<TheController>(() => new TheController());
        private readonly ActorSystem clientActorSystem;
        private readonly IActorRef newTorrentClientActorRef;

        public void Add(string filePath)
        {
            newTorrentClientActorRef.Tell(new NewTorrentCommand(filePath));
        }

        public void Dispose()
        {
            clientActorSystem?.Dispose();
        }
    }
}