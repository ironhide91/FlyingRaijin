using Akka.Actor;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Text;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
    internal class PieceWriterActorBuilder : ActorBuilderBase<MetaData, ChannelReader<PieceMessage>, ChannelWriter<CompletePiece>>
    {
        internal override IActorRef Build(IUntypedActorContext context)
        {
            return context.ActorOf(Props.Create(() => new PieceWriterActor(Value1, Value2, Value3)));
        }
    }

    internal class PieceWriterActor : ReceiveActor, IWithTimers
    {
        internal PieceWriterActor(
            MetaData torrent,
            ChannelReader<PieceMessage> pieceMessageReader,
            ChannelWriter<CompletePiece> globalPieceBlockWriter)
        {
            this.torrent = torrent;
            this.pieceMessageReader = pieceMessageReader;
            this.globalPieceBlockWriter = globalPieceBlockWriter;

            pieceDictionary = new PieceDictionary((int)torrent.PieceLength);

            Receive<MonitorPipeForData>(_ => OnMonitorChannelReaderForData());
        }

        private readonly MetaData torrent;
        private readonly ChannelReader<PieceMessage> pieceMessageReader;
        private readonly ChannelWriter<CompletePiece> globalPieceBlockWriter;
        private PieceDictionary pieceDictionary;

        public ITimerScheduler Timers { get; set; }

        protected override void PreStart()
        {
            Timers.StartPeriodicTimer(
                nameof(MonitorChannelReaderForData),
                MonitorChannelReaderForData.Instance,
                TimeSpan.FromSeconds(60),
                TimeSpan.FromSeconds(20));
        }

        private void OnMonitorChannelReaderForData()
        {
            int maxMessageToProcess = 10;

            while (maxMessageToProcess != 0 && pieceMessageReader.TryRead(out PieceMessage message))
            {
                pieceDictionary.Add(message);

                foreach (var piece in pieceDictionary.Pieces)
                {
                    if (piece.Value.PendingPieceLength > 0)
                    {
                        continue;
                    }

                    // we got all blocks in this piece
                    // now compute hash
                    var bytes = ByteArrayPool.Rent(20);
                    Span<byte> hash = bytes;
                    Bencode.BencodeEngine.GenerateHash(piece.Value.Buffer.Span, hash);

                    // should match hash from torrent metadata
                    if (torrent.PieceHash.Sha1Checksums[piece.Key].Span == hash)
                    {
                        var sb = new StringBuilder();

                        for (int i = 0; i < hash.Length; i++)
                        {
                            sb.Append(hash[i].ToString("X2"));
                        }

                        // send to GlobalPieceWriter
                        globalPieceBlockWriter.TryWrite(new CompletePiece(torrent, message.Index, piece.Value));
                    }

                    ByteArrayPool.Return(bytes);
                }

                --maxMessageToProcess;
            }            
        }
    }
}