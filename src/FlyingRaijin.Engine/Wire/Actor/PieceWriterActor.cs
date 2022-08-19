using Akka.Actor;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Text;
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

        public ITimerScheduler Timers { get; set; }

        protected override void PreStart()
        {
            Timers.StartPeriodicTimer(
                nameof(MonitorChannelReaderForData),
                MonitorChannelReaderForData.Instance,
                TimeSpan.FromSeconds(60),
                TimeSpan.FromSeconds(10));
        }

        private void OnMonitorChannelReaderForData()
        {
            int maxMessageToProcess = 10;

            while (maxMessageToProcess != 0 && pieceMessageReader.TryRead(out PieceMessage message))
            {
                IRequestPieceBlock pieceBlock = pieceDictionary;

                pieceBlock.Request(message.Index, out PieceBlock requestedBlock);

                if (requestedBlock.IsPending)
                {
                    continue;
                }

                // we got all blocks in this piece
                // now compute hash
                var bytes = ByteArrayPool.Rent(20);
                Span<byte> hash = bytes;
                BencodeEngine.GenerateHash(requestedBlock.Buffer.Span, hash);

                // should match hash from torrent metadata
                if (torrent.PieceHash.Sha1Checksums[requestedBlock.Index].Span == hash)
                {
                    var sb = new StringBuilder();

                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }

                    PieceMessagePoolPolicy.Instance.Return(message);

                    // send to GlobalPieceWriter
                    globalPieceBlockWriter.TryWrite(new CompletePiece(torrent, message.Index, requestedBlock));
                }

                ByteArrayPool.Return(bytes);

                --maxMessageToProcess;
            }            
        }

        private readonly MetaData torrent;
        private readonly ChannelReader<PieceMessage> pieceMessageReader;
        private readonly ChannelWriter<CompletePiece> globalPieceBlockWriter;
        private PieceDictionary pieceDictionary;
    }

    internal class PieceWriterActorBuilder : ActorBuilderBase<MetaData, ChannelReader<PieceMessage>, ChannelWriter<CompletePiece>>
    {
        internal PieceWriterActorBuilder()
        {
            ctor = Props.Create(() => new PieceWriterActor(Value1, Value2, Value3));
        }
    }
}