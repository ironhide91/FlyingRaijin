using Akka.Actor;
using FlyingRaijin.Engine.Tracker;
using FlyingRaijin.Messages;
using System;
using System.Buffers;
using System.Net.Http;

namespace FlyingRaijin.Engine.Actors
{
    public class TrackerActor : ReceiveActor
    {
        public static Props Props(
            string announceUrl,
            byte[] infoHash,
            string peerId,
            string ip,
               int port)
        {
            var actor = Akka.Actor.Props.Create(() =>
                new TrackerActor(
                        announceUrl,
                        infoHash,
                        peerId,
                        ip,
                        port));

            return actor;
        }

        public readonly string announceUrl;
        public readonly byte[] infoHash;
        public readonly string peerId;
        public readonly string ip;
        public readonly    int port;

        public readonly TrackerRequestBuilder trackerRequestBuilder;

        private readonly ActorSelection httpClientActorRef;

        public TrackerActor(
            string announceUrl,
            byte[] infoHash,
            string peerId,
            string ip,
               int port)
        {
            this.announceUrl = announceUrl;
               this.infoHash = infoHash;
                     this.ip = ip;
                   this.port = port;

            trackerRequestBuilder =
                new TrackerRequestBuilder(announceUrl)
                        .WithInfoHash(infoHash)
                        .WithPeerId(peerId)
                        .WithIP(ip)
                        .WithPort(port);

            httpClientActorRef = Context.ActorSelection("/user/HttpClientActor");

                Receive<AnnounceCommand>(command => OnAnnounceCommand(command));
            Receive<HttpResponseMessage>(message => OnHttpResponseMessage(message));
        }

        private void OnAnnounceCommand(AnnounceCommand command)
        {
            var url =
                trackerRequestBuilder
                    .WithUploaded(0)
                    .WithDownloaded(0)
                    .WithLeft(0)
                    .WithEvent(EventType.Started)
                    .WithCompact(1)
                    .Build();

            System.Diagnostics.Debug.WriteLine(url);
            httpClientActorRef.Tell(new HttpGetCommand(url));
        }

        private void OnHttpResponseMessage(HttpResponseMessage message)
        {
            if (message.StatusCode != System.Net.HttpStatusCode.OK)
            {

            }

            //System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(message.Content.ReadAsByteArrayAsync().Result));

            var stream = message.Content.ReadAsStream();
            int length = (int)stream.Length; 
            var rawBuffer = ArrayPool<byte>.Shared.Rent(length);
            Span<byte> spanBuffer = rawBuffer;
            stream.Read(spanBuffer);

            if (TrackerResponseParser.TryParse(spanBuffer.Slice(0, length), out TrackerResponse response))
            {
                Context.Parent.Tell(response);
            }

            //error
        }
    }
}