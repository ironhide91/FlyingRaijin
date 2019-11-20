using FlyingRaijin.Bencode.Ast.Base;
using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Ast
{
    public sealed class TorrentRoot : NonTerminalNodeBase
    {
        public TorrentRoot()
        {
            Children = new List<NodeBase>();
        }

        public override Production ProductionType => Production.TORRENT;

        public override ICollection<NodeBase> Children { get; }
    }
}