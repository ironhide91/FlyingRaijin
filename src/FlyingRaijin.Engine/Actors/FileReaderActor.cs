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
            var handle = File.OpenHandle(command.Path);
            var length = RandomAccess.GetLength(handle);

            var file = new FileRead();
            file.InitializeBuffer((int)length);
            RandomAccess.Read(handle, file.Buffer, 0);            
            handle.Close();

            Sender.Tell(file);
        }
    }
}