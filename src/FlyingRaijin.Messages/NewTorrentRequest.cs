namespace FlyingRaijin.Messages
{
    public class NewTorrentRequest
    {
        public NewTorrentRequest(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; private set; }
    }
}