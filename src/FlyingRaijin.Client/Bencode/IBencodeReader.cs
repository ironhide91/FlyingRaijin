using FlyingRaijin.Client.Torrent;
using System.Text;

namespace FlyingRaijin.Client.Bencode
{
    public interface IBencodeReader
    {
        ITorrent Read(Encoding encoding, string bencodeValue);
    }
}