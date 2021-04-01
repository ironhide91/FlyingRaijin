using Akka.Actor;
using FlyingRaijin.Messages;
using System.Collections.Generic;

namespace FlyingRaijin.Controller.Actors
{
    public class NewTorrentClientActor : ReceiveActor
    {
        public NewTorrentClientActor()
        {
            Receive<NewTorrentCommand>(request => NewTorrentRequestHandler(request));
            Receive<NewTorrentResponse>(response => NewTorrentResponseHandler(response));
        }

        private void NewTorrentRequestHandler(NewTorrentCommand request)
        {
            //Sender.Tell(new NewTorrentCommand(request.FilePath));
            var engine = Context.ActorSelection("akka.tcp://Engine@localhost:8085/user/NewTorrentActor");
            engine.Tell(request);
        }

        private void NewTorrentResponseHandler(NewTorrentResponse response)
        {

        }

        //private readonly Dictionary<IActorRef> clients = new HashSet<IActorRef>();
    }
}