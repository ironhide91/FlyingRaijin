using Akka.Actor;
using FlyingRaijin.Engine.Messages;
using FlyingRaijin.Messages;

namespace FlyingRaijin.Engine.Actors
{
    internal class NewTorrentActor : ReceiveActor
    {
        public NewTorrentActor()
        {
            Receive<NewTorrentCommand>(command => OnNewTorrentCommand(command));
        }

        private void OnNewTorrentCommand(NewTorrentCommand command)
        {
            var torrentManger =
                Context.ActorOf(TorrentManagerActor.Props(command.FilePath));

            torrentManger.Tell(new BeginCommand());

            //FSM<State, IData>
        }
    }
}