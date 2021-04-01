using Akka.Actor;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Tracker;
using FlyingRaijin.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FlyingRaijin.Engine.Actors
{
    public class TorrentManagerActor : ReceiveActor
    {
        public static Props Props(string path)
        {
            return Akka.Actor.Props.Create(() => new TorrentManagerActor(path));
        }

        private readonly string torrentPath;

        private readonly HashSet<DnsEndPoint> peers = new HashSet<DnsEndPoint>(30);

        private MetaData torrent;

        private IActorRef peerManager;

        public TorrentManagerActor(string path)
        {
            torrentPath = path;

               Receive<BeginCommand>(command => OnBeginCommand());
                   Receive<FileRead>(message => OnFileRead(message));
                   Receive<MetaData>(message => OnMetaData(message));
            Receive<TrackerResponse>(message => OnTrackerResponse(message));
        }

        private void OnBeginCommand()
        {
            Context.ActorOf<FileReaderActor>().Tell(new FileReadCommand(torrentPath));
        }

        private void OnFileRead(FileRead message)
        {
            Context.ActorOf<ParseActor>().Tell(message);
        }

        private void OnMetaData(MetaData message)
        {
            torrent = message;

            var trackerActor = Context.ActorOf(
                TrackerActor.Props(
                    message.AnnounceUrl,
                    message.InfoHash.ToArray(),
                    "ABCDEFGHIJKLMNOPQRST",
                    "255.255.255.255",
                    63499));

            trackerActor.Tell(new AnnounceCommand());
        }

        private void OnTrackerResponse(TrackerResponse message)
        {
            if (peerManager == null)
                peerManager = Context.ActorOf(PeerManagerActor.Props(torrent, message.Peers));

            peerManager.Tell(new NewPeers(message.Peers));
        }
    }
}