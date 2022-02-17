using Akka.Actor;
using FlyingRaijin.Engine.Messages;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.IO;

namespace FlyingRaijin.Engine.Actors
{
    internal class FileCreateActor : ReceiveActor
    {
        internal FileCreateActor()
        {
            Receive<FileCreate>(command => OnFileCreate(command));
        }

        private readonly Dictionary<string, SafeFileHandle> openFileHandles =
            new Dictionary<string, SafeFileHandle>(100);

        private void OnFileCreate(FileCreate file)
        {
            if (openFileHandles.ContainsKey(file.Path))
            {

            }

            var handle = File.OpenHandle(
                file.Path,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.Write,
                FileOptions.RandomAccess);

            openFileHandles.Add(file.Path, handle);
        }
    }
}