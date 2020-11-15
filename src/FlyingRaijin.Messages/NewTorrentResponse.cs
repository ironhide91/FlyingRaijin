namespace FlyingRaijin.Messages
{
    public class NewTorrentResponse
    {
        public NewTorrentResponse(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; private set; }
    }
}