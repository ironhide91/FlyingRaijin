using Akka.Actor;
using FlyingRaijin.Messages;

namespace FlyingRaijin.Engine.ActorModel.Actors
{
    internal class NewTorrentEngineActor : ReceiveActor
    {
        public NewTorrentEngineActor()
        {
            Receive<NewTorrentRequest>(request => Process(request));
        }

        private void Process(NewTorrentRequest request)
        {
            Sender.Tell(new NewTorrentResponse(0L));
        }
    }
}