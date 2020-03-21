using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using System.Linq;

namespace FlyingRaijin.Bencode.Read.Converter
{
    internal static class NumberConverter
    {
        internal static byte[] Convert(NodeBase node)
        {
            var numberNode = node.CastOrThrow<NumberNode>();

            return numberNode.Children.Cast<TerminalByteNodeBase>().Select(x => x.Byte).ToArray();
        }
    }
}