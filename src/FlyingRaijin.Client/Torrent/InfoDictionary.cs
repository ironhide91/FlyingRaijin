using FlyingRaijin.Bencode.Read.ClrObject;
using System;
using System.Collections.Generic;

namespace FlyingRaijin.Client.Torrent
{
    public sealed class InfoDictionary
    {
        public InfoDictionary(IReadOnlyDictionary<string ,IClrObject> infoDictionary)
        {
            Values = infoDictionary ?? throw new ArgumentNullException(nameof(infoDictionary));

            //- Set Piece Length
            //PieceLength = Values[PIECE_LENGTH_KEY].Cast<int>();

            ////- Set  Private flag
            //IsPrivate = (Values[PIECE_LENGTH_KEY].Cast<int>() == 1);
        }

        

        public readonly IReadOnlyDictionary<string, IClrObject> Values;

        public readonly int PieceLength;

        public readonly bool IsPrivate;

    }
}