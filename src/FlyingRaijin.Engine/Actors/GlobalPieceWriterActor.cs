using Akka.Actor;
using FlyingRaijin.Engine.Messages;
using FlyingRaijin.Engine.Torrent;
using FlyingRaijin.Engine.Wire;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
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
            Receive<FileCreated>(message => OnFileCreated(message));
        }        

        private const long MaxBytesToWrite = 512000L;
        private readonly ChannelReader<CompletePiece> channelReaderPiece;
        private readonly Timer timer;

        private void OnStartPieceWriterThread()
        {
            if (timer.Enabled)
                return;

            timer.Start();
        }

        private void OnFileCreated(FileCreated message)
        {
            FileManager.Add(message.InfoHash, message.File, message.FileHandle);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            long maxBytesWritten = 0;

            while ((maxBytesWritten <= MaxBytesToWrite) && channelReaderPiece.TryRead(out CompletePiece piece))
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
    }
}