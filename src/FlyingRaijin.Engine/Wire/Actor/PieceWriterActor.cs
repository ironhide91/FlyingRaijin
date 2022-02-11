using Akka.Actor;
using System;
using System.Threading.Channels;

namespace FlyingRaijin.Engine.Wire
{
    internal class SingleFilePieceWriterActor : ReceiveActor, IWithTimers
    {
        internal static Props Props(ChannelReader<ReadOnlyMemory<byte>> channelReader)
        {
            return Akka.Actor.Props.Create(() => new SingleFilePieceWriterActor(channelReader));
        }

        private SingleFilePieceWriterActor(ChannelReader<ReadOnlyMemory<byte>> channelReader)
        {
            this.channelReader = channelReader;

            Receive<MonitorPipeForData>(_ => OnMonitorChannelReaderForData());
        }        

        private readonly ChannelReader<ReadOnlyMemory<byte>> channelReader;

        public ITimerScheduler Timers { get; set; }

        protected override void PreStart()
        {
            Timers.StartPeriodicTimer(
                nameof(MonitorChannelReaderForData),
                MonitorChannelReaderForData.Instance,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5));
        }

        private void OnMonitorChannelReaderForData()
        {

        }
    }
}