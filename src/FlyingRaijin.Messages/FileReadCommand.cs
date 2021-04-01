namespace FlyingRaijin.Messages
{
    public class FileReadCommand
    {
        public readonly string Path;

        public FileReadCommand(string path)
        {
            Path = path;
        }
    }
}