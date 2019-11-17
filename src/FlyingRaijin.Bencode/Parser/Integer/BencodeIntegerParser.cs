using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Integer;

namespace FlyingRaijin.Bencode.Parser
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