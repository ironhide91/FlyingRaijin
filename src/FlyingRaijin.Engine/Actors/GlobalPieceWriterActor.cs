﻿using Akka.Actor;
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

            timer = new Timer(30000);
            timer.Elapsed += Timer_Elapsed;
            
            Receive<StartPieceWriterThread>(_ => OnStartPieceWriterThread());
            Receive<FileCreated>(message => OnFileCreated(message));
        }        

        private const int MaxBytesToWrite = 512 * 10;
        private readonly ChannelReader<CompletePiece> channelReaderPiece;
        private readonly Dictionary<string, SafeFileHandle> openFileHandles;
        private readonly Timer timer;

        public ITimerScheduler Timers { get; set; }

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
            long maxBytesWritten = MaxBytesToWrite;

            while (maxBytesWritten != 0 && channelReaderPiece.TryRead(out CompletePiece piece))
            {
                //var (overlaps, file) = PieceOverlaps(piece);

                //ReadOnlySpan<byte> bufferToWrite;

                //if (overlaps)
                //{
                //    if (file.PieceIndexBegin == piece.PieceIndex)
                //    {

                //    }

                //    var length = file.PieceIndexBegin - piece.PieceIndex;

                //    bufferToWrite = piece.Block.Buffer.Span.Slice(file.PieceIndexBegin, length);
                //}
                //else
                //{
                //    bufferToWrite = piece.Block.Buffer.Span;
                //}

                //// can write whole piece as part of a single file

                //var exist = FileManager.TryGet(
                //    piece.MetaData.InfoHash,
                //    file.Path,
                //    out SafeFileHandle fileHandle);

                //if (!exist)
                //{
                //    continue;
                //}

                //var offset = (piece.PieceIndex - file.PieceIndexBegin);

                //try
                //{
                //    RandomAccess.Write(fileHandle, bufferToWrite, offset);
                //}
                //catch (Exception ex)
                //{

                //}
                //finally
                //{
                //    piece.Block.ReleaseBuffer();
                //}
            }
        }

        
    }
}