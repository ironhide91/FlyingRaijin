namespace FlyingRaijin.Messages
{
    public class ReadFileCommand
    {
        public readonly string Path;

        public ReadFileCommand(string path)
        {
            Path = path;
        }
    }
}