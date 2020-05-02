using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Ast.Base
{
    public abstract class TerminalCharNodeBase : NodeBase
    {
        private static Encoding UTF8NoBOM = new UTF8Encoding(false, false);

        private static ImmutableList<NodeBase> Empty => ImmutableList.Create<NodeBase>();      

        public override IList<NodeBase> Children { get => Empty; }

        public abstract char Character { get; }

        public static char ToUTF8Char(string character)
        {
            return char.Parse(UTF8NoBOM.GetString(UTF8NoBOM.GetBytes(character)));
        }
    }
}