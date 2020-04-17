using FlyingRaijin.Bencode.Read.ClrObject;
using System.Collections.Immutable;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class MultiFileInfoDictionary : InfoDictionaryBase
    {
        public MultiFileInfoDictionary(BDictionary infoDictionary)
        {
                  _Pieces = infoDictionary.ReadPieces();
               _IsPrivate = infoDictionary.ReadIsPrivateFlag();
             _PieceLength = infoDictionary.ReadPieceLength();
            DirectoryName = infoDictionary.ReadMultiName();
                    Files = infoDictionary.ReadMultiFiles();
        }

        private readonly long _PieceLength;
        private readonly bool _IsPrivate;
        private readonly Pieces _Pieces;        

        public override bool IsPrivate => _IsPrivate;
        public override long PieceLength => _PieceLength;
        public override Pieces Pieces => _Pieces;

        public readonly string DirectoryName;
        public readonly ImmutableList<MultiFileInfoDictionaryItem> Files;
    }
}