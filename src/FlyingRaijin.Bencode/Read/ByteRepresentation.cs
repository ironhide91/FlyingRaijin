using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        private const byte dictStart = (byte)'d';
        private const byte listStart = (byte)'l';
        private const byte intStart  = (byte)'i';
        private const byte end       = (byte)'e';
        private const byte colon     = (byte)':';
        private const byte zero      = (byte)'0';
        private const byte minus     = (byte)'-';

        private static readonly HashSet<byte> NonZeroIntegerBytes = new HashSet<byte>
        {
            (byte)'-',
            (byte)'1',
            (byte)'2',
            (byte)'3',
            (byte)'4',
            (byte)'5',
            (byte)'6',
            (byte)'7',
            (byte)'8',
            (byte)'9',
        };

        private static readonly HashSet<byte> PositiveIntegerBytes = new HashSet<byte>
        {
            (byte)'0',
            (byte)'1',
            (byte)'2',
            (byte)'3',
            (byte)'4',
            (byte)'5',
            (byte)'6',
            (byte)'7',
            (byte)'8',
            (byte)'9',
        };
    }
}