using Akka.Actor;
using FlyingRaijin.Engine.Messages;
using FlyingRaijin.Engine.Wire;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Threading.Channels;
using System.Timers;

namespace FlyingRaijin.Engine
{
    internal class GlobalPieceWriterActor : ReceiveActor
    {
        internal static Props Props(ChannelReader<CompletePiece> channelReader)
        {
            return Akka.Actor.Props.Create(() => new GlobalPieceWriterActor(channelReader));
        }

        private GlobalPieceWriterActor(ChannelReader<CompletePiece> channelReaderPiece)
        {
            this.channelReaderPiece = channelReaderPiece;

            timer = new Timer(40000);
            timer.Elapsed += Timer_Elapsed;
            
            Receive<StartPieceWriterThread>(_ => OnStartPieceWriterThread());
        }

        private void OnStartPieceWriterThread()
        {
            if (timer.Enabled)
                return;

            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            long maxBytesWritten = 0;

            while ((maxBytesWritten <= MaxBytesPerWrite) && channelReaderPiece.TryRead(out CompletePiece piece))
            {
                var writeInfos = PieceFileHelper.GetWriteInfo(piece);

                foreach (var (file, sliceInfo) in writeInfos)
                {
                    var exist = FileManager.TryGet(
                        piece.MetaData.InfoHash,
                        file.Path,
                        out SafeFileHandle fileHandle);

                    if (!exist)
                    {
                        continue;
                    }

                    var bufferToWrite = piece.Block.Buffer.Span.Slice(sliceInfo.Start, sliceInfo.Length);

                    try
                    {
                        RandomAccess.Write(fileHandle, bufferToWrite, sliceInfo.Offset);                        
                        maxBytesWritten += piece.MetaData.PieceLength;
                        piece.Block.ReleaseBuffer();
                    }
                    catch (Exception ex)
                    {

                    }
                }                
            }
        }

        private const long MaxBytesPerWrite = 512000L;
        private readonly ChannelReader<CompletePiece> channelReaderPiece;
        private readonly Timer timer;
    }
}