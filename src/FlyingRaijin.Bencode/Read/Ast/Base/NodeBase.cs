using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Read.Ast.Base
{
    public abstract class NodeBase
    {
        public abstract Production ProductionType { get; }

        public abstract IList<NodeBase> Children { get; }

        public void TreeTextRepresentation()
        {
            
        }
    }
}