using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Ast.Base
{
    public abstract class NonTerminalNodeBase : NodeBase
    {
        protected NonTerminalNodeBase()
        {
            Children = new List<NodeBase>();
        }

        public override IList<NodeBase> Children { get; }
    }
}