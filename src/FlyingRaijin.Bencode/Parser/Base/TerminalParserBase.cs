using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Parser.Base
{
    public abstract class TerminalParserBase<T> : IParser where T : TerminalByteNodeBase
    {
        public abstract Production ProductionType { get; }

        public abstract void Parse(ParseContext context, NodeBase ast);
    }     
}