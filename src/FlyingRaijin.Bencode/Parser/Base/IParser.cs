using FlyingRaijin.Bencode.Ast.Base;

namespace FlyingRaijin.Bencode.Parser.Base
{
    public interface IParser
    {
        Production ProductionType { get; }

        void Parse(ParseContext context, NodeBase ast);
    }
}