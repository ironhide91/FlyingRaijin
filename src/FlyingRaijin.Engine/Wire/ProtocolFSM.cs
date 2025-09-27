using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRaijin.Engine.Wire
{
    internal enum ProtocolState
    {
        Choke,
        Unchoke,
        Interested,
        NotInterested,
        Have,
        BitField,
        Request,
        Piece,
        Cancel,
    }

    

    //internal class ProtocolFSM : FSM<ProtocolState, IData>
    //{
    //    public ProtocolFSM()
    //    {
    //        StartWith(ProtocolState.Choke, default);
    //    }
    //}
}