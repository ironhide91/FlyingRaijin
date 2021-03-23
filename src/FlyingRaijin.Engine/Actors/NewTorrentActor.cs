using Akka.Actor;
using FlyingRaijin.Messages;

namespace FlyingRaijin.Engine.Actors
{
    internal class NewTorrentActor : ReceiveActor
    {
        public NewTorrentActor()
        {
            Receive<NewTorrentCommand>(request => OnNewTorrentRequest(request));
        }

        private void OnNewTorrentRequest(NewTorrentCommand request)
        {
            var torrentManger = Context.ActorOf(TorrentManagerActor.Props(request));
            //torrentManger.Tell(new StartParsingCommand());

            //Sender.Tell(new NewTorrentResponse(0L));
        }
    }
}