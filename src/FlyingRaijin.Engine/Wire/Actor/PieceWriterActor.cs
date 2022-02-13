using Akka.Actor;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
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
            while (pieceMessageReader.TryRead(out PieceMessage message))
            {
                pieceDictionary.Add(message);
            }

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
                    // send to GlobalPieceWriter
                    globalPieceBlockWriter.TryWrite(new CompletePiece(torrent, piece.Value));
                }
            }
        }
    }
}