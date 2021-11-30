namespace FlyingRaijin.Messages
{
    public class NewTorrentCommand
    {
        public NewTorrentCommand(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; private set; }
    }
}