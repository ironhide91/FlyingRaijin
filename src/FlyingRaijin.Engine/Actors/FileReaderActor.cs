using Akka.Actor;
using FlyingRaijin.Engine.Messages;
using System.IO;

namespace FlyingRaijin.Engine.Actors
{
    public class FileReaderActor : ReceiveActor
    {
        public FileReaderActor()
        {
            Receive<FileReadCommand>(command => OnReadFileCommand(command));
        }

        private void OnReadFileCommand(FileReadCommand command)
        {
            var data = File.ReadAllBytes(command.Path);

            var fileRead = new FileRead(data);

            Sender.Tell(fileRead);
        }
    }
}