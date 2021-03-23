using Akka.Actor;
using FlyingRaijin.Messages;

namespace FlyingRaijin.Engine.Actors
{
    public class TorrentManagerActor : ReceiveActor
    {
        public static Props Props(NewTorrentCommand command)
        {
            return Akka.Actor.Props.Create(() => new TorrentManagerActor(command));
        }

        public TorrentManagerActor(NewTorrentCommand command)
        {
            //Receive<StartParsingCommand>(command => OnStartParsingCommand(command));
        }
    }
}