using Akka.Actor;
using Akka.IO;
using System;
using static Akka.IO.Tcp;

namespace FlyingRaijin.Engine.Wire
{
    internal interface ITcpData
    {

    }

    internal enum TcpState
    {
        Void,
        Connecting,
        ConnectionFailed,
        ConnectionClosed,
        Connected,
        Received,
        Disconnected
    }

    internal class TcpFSM : FSM<TcpState, ITcpData>
    {
        public TcpFSM()
        {
            StartWith(TcpState.Void, default);

            When(TcpState.Connecting, state => OnConnecting(state));
            When(TcpState.Connected, state => OnConnected(state));
        }

        private State<TcpState, ITcpData> OnConnecting(Event<ITcpData> state)
        {
            Context.System.Tcp().Tell(new Connect(null));

            var temp = Context.System.Tcp();

            //GoTo(tcp)

            //this.GoTo

            return null;
        }

        private State<TcpState, ITcpData> OnConnected(Event<ITcpData> state)
        {
            return null;
        }
    }
}