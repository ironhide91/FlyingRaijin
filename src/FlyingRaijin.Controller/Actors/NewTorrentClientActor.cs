using Akka.Actor;
using FlyingRaijin.Messages;
using System.Collections.Generic;

namespace FlyingRaijin.Controller.Actors
{
    public class NewTorrentClientActor : ReceiveActor
    {
        public NewTorrentClientActor()
        {
            Receive<NewTorrentRequest>(request => NewTorrentRequestHandler(request));
            Receive<NewTorrentResponse>(response => NewTorrentResponseHandler(response));
        }

        private void NewTorrentRequestHandler(NewTorrentRequest request)
        {
            Sender.Tell(new NewTorrentRequest("alex"));
            var engine = Context.ActorSelection("akka.tcp://Engine@localhost:8085/user/NewTorrentEngineActor");
            engine.Tell(request);
        }

        private void NewTorrentResponseHandler(NewTorrentResponse response)
        {

        }

        //private readonly Dictionary<IActorRef> clients = new HashSet<IActorRef>();
    }
}