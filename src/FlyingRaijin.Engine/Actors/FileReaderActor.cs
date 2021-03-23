using Akka.Actor;
using FlyingRaijin.Messages;
using System.IO;

namespace FlyingRaijin.Engine.Actors
{
    public class FileReaderActor : ReceiveActor
    {
        public FileReaderActor()
        {
            Receive<ReadFileCommand>(command => OnReadFileCommand(command));
        }

        private void OnReadFileCommand(ReadFileCommand command)
        {
            var data = File.ReadAllBytes(command.Path);

            var fileRead = new FileRead(data);

            Sender.Tell(fileRead);
        }
    }
}