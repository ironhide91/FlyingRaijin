using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Integer;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void BencodeIntegerParser(ParseContext context, NodeBase ast)
        {
            var node = new BencodeIntegerNode();
            ast.Children.Add(node);

            IntegerStartParser(context, node);
            IntegerParser(context, node);
            EndParser(context, node);
        }
    }
}