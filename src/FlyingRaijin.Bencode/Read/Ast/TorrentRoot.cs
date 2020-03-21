using FlyingRaijin.Bencode.Read.Ast.Base;
using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Read.Ast
{
    public sealed class TorrentRoot : NonTerminalNodeBase
    {
        public TorrentRoot()
        {
            Children = new List<NodeBase>();
        }

        public override Production ProductionType => Production.TORRENT;

        public override IList<NodeBase> Children { get; }
    }
}