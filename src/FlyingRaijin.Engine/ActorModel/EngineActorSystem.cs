using Akka.Actor;
using FlyingRaijin.Engine.ActorModel.Actors;
using System;

namespace FFlyingRaijin.Engine.ActorModel
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
            newTorrentEngineActorRef = engineActorSystem.ActorOf<NewTorrentEngineActor>(nameof(NewTorrentEngineActor));
        }

        internal void Stop()
        {
            engineActorSystem.Dispose();
        }
    }
}