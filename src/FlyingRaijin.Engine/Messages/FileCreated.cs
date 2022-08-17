using FlyingRaijin.Engine.Torrent;
using Microsoft.Win32.SafeHandles;

namespace FlyingRaijin.Engine.Messages
{
    internal class FileCreated
    {
        internal FileCreated(InfoHash infoHash, string file, SafeFileHandle handle)
        {
            InfoHash = infoHash;
            File = file;
            FileHandle = handle;
        }

        internal readonly InfoHash InfoHash;
        internal readonly string File;
        internal readonly SafeFileHandle FileHandle;
    }
}