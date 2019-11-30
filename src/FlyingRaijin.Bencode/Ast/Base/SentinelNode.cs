using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlyingRaijin.Bencode.Ast.Base
{
    public sealed class SentinelNode : NodeBase
    {
        public override Production ProductionType => Production.Sentinel;

        public override IList<NodeBase> Children =>
            new ReadOnlyCollection<NodeBase>(Enumerable.Empty<NodeBase>().ToList());
    }
}
