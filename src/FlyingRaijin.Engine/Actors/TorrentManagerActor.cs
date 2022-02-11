using Akka.Actor;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Tracker;
using FlyingRaijin.Engine.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Channels;
using FlyingRaijin.Engine.Wire;

namespace FlyingRaijin.Engine.Actors
{

    internal class PieceChannel : Channel<ReadOnlyMemory<byte>>
    {

    }

    public class TorrentManagerActor : ReceiveActor
    {
        private static readonly ReadOnlyMemory<byte> peerId = Encoding.UTF8.GetBytes("ABCDEFGHIJKLMNOPQR15");

        public static Props Props(string path)
        {
            return Akka.Actor.Props.Create(() => new TorrentManagerActor(path));
        }

        private readonly string torrentPath;
        private readonly Channel<ReadOnlyMemory<byte>> channel;
        private readonly HashSet<DnsEndPoint> peers = new HashSet<DnsEndPoint>(30);

        private MetaData torrent;

        private IActorRef peerManager;
        private IActorRef pieceWriter;

        public TorrentManagerActor(string path)
        {
            torrentPath = path;

            channel = Channel.CreateUnbounded<ReadOnlyMemory<byte>>(
                new UnboundedChannelOptions()
                {
                    SingleReader = true,
                    SingleWriter = false
                });

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

            peerManager = Context.ActorOf(PeerManagerActor.Props(peerId, torrent));
            pieceWriter = Context.ActorOf(SingleFilePieceWriterActor.Props(channel.Reader));

            trackerActor.Tell(new AnnounceCommand());
        }

        private void OnTrackerResponse(TrackerResponse message)
        {                

            peerManager.Tell(new NewPeers(message.Peers));
        }
    }
}