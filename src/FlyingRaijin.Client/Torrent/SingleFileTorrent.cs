using System;
using FlyingRaijin.Bencode.Read.ClrObject;

namespace FlyingRaijin.Client.Torrent
{
    public sealed class SingleFileTorrent : ITorrent
    {
        public SingleFileTorrent(BDictionary dictionary)
        {
            Initilaize(dictionary);
        }

        private void Initilaize(BDictionary dictionary)
        {
            //if (dictionary == null)
            //    throw new ArgumentNullException(nameof(dictionary));

            //- Required
            AnnounceUrl  = new Uri(MetaInfoHelper.ReadComment(dictionary));
            //- Optional
            CreationDate = dictionary.ReadCreationDate().FromUnixTime();
                 Comment = dictionary.ReadComment();
               CreatedBy = dictionary.ReadCreatedBy();
                Encoding = dictionary.ReadEncoding();

             PieceLength = dictionary.ReadPieceLength();
            //pie = dictionary.re;
            IsPrivate = dictionary.ReadPrivateFlag();
        }

        //- ITorrent
        public Uri AnnounceUrl { get; private set; }
        public AnnounceList AnnounceList { get; private set; }
        public DateTime? CreationDate { get; private set; }
        public string Comment { get; private set; }
        public string CreatedBy { get; private set; }
        public string Encoding { get; private set; }
        public int PieceLength { get; private set; }
        public bool IsPrivate { get; private set; }
        //- Single File
        public string Name { get; private set; }
        public long Length { get; private set; }
        public string Md5checkSum { get; private set; }
    }
}