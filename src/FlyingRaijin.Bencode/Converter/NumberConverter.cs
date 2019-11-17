using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.Ast.Shared;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Converter
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