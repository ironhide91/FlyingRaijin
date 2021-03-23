using Akka.Actor;
using FlyingRaijin.Engine.Actors;
using System;

namespace FFlyingRaijin.Engine.Actors
{
    internal class EngineActorSystem
    {
        public static EngineActorSystem Instance { get { return lazy.Value; } }

        private static readonly Lazy<EngineActorSystem> lazy =
            new Lazy<EngineActorSystem>(() => new EngineActorSystem());

        private ActorSystem engineActorSystem;
        private IActorRef newTorrentEngineActorRef;        

        private EngineActorSystem()
        {
            
        }

        internal void Start()
        {
            engineActorSystem = ActorSystem.Create("Engine");
            newTorrentEngineActorRef = engineActorSystem.ActorOf<NewTorrentActor>(nameof(NewTorrentActor));
        }

        internal void Stop()
        {
            engineActorSystem.Dispose();
        }
    }
}