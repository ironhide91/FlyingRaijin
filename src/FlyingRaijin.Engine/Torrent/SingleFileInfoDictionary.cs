using FlyingRaijin.Bencode.BObject;

namespace FlyingRaijin.Engine.Torrent
{
    public sealed class SingleFileInfoDictionary : InfoDictionaryBase
    {
        public SingleFileInfoDictionary(BDictionary infoDictionary)
        {            
                 _PieceLength = infoDictionary.ReadPieceLength();
                      _Pieces = infoDictionary.ReadPieces();
                   _IsPrivate = infoDictionary.ReadIsPrivateFlag();
                     FileName = infoDictionary.ReadSingleName();
            FileLengthInBytes = infoDictionary.ReadSingleLength();
                  MD5Checksum = infoDictionary.ReadSingleMD5Checksum();
        }

        private readonly long _PieceLength;        
        private readonly Pieces _Pieces;
        private readonly bool _IsPrivate;        
        
        public override long PieceLength => _PieceLength;
        public override Pieces Pieces => _Pieces;
        public override bool IsPrivate => _IsPrivate;

        public readonly string FileName;
        public readonly long FileLengthInBytes;
        public readonly string MD5Checksum;
    }
}