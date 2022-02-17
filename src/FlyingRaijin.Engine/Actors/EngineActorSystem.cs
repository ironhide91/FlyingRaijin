using Akka.Actor;
using Akka.Streams;
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
        private IActorRef newTorrentActor;
        private IActorRef httpClientActor;
        private IActorRef fileCreateActor;
        private IActorRef globalPieceWriterActor;

        private EngineActorSystem()
        {
            
        }

        internal void Start()
        {
            engineActorSystem = ActorSystem.Create("Engine");

            newTorrentActor = engineActorSystem.ActorOf<NewTorrentActor>(nameof(NewTorrentActor));
            httpClientActor = engineActorSystem.ActorOf<HttpClientActor>(nameof(HttpClientActor));
            httpClientActor = engineActorSystem.ActorOf<HttpClientActor>(nameof(HttpClientActor));
            httpClientActor = engineActorSystem.ActorOf<HttpClientActor>(nameof(HttpClientActor));
        }

        internal void Stop()
        {
            engineActorSystem.Dispose();
        }
    }
}