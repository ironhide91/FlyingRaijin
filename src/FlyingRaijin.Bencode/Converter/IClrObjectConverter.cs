﻿using FlyingRaijin.Bencode.Ast.Base;
using FlyingRaijin.Bencode.ClrObject;
using System.Text;

namespace FlyingRaijin.Bencode.Converter
{
    public interface IClrObjectConverter<N, T>
        where N : NonTerminalNodeBase
        where T : IClrObject
    {
        T Convert(Encoding encoding, N node);
    }
}