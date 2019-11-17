using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Ast.Base
{
    public abstract class NodeBase
    {
        public abstract Production ProductionType { get; }

        public abstract ICollection<NodeBase> Children { get; }

        public void TreeTextRepresentation()
        {
            
        }
    }
}