using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.ClrObject;
using System.Text;

namespace FlyingRaijin.Bencode.Read.Converter
{
    public interface IClrObjectConverter<N, T>
        where N : NonTerminalNodeBase
        where T : IClrObject
    {
        T Convert(Encoding encoding, N node);
    }
}