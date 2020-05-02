using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.Shared;
using System.Linq;

namespace FlyingRaijin.Bencode.Read.Converter
{
    internal static class NumberConverter
    {
        internal static char[] Convert(NodeBase node)
        {
            var numberNode = (NumberNode)node;

            return numberNode.Children.Cast<TerminalCharNodeBase>().Select(x => x.Character).ToArray();
        }
    }
}