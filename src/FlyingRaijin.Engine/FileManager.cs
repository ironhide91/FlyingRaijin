using FlyingRaijin.Engine.Torrent;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;

namespace FlyingRaijin.Engine
{
    internal static class FileManager
    {
        private static readonly Dictionary<InfoHash, Dictionary<string, SafeFileHandle>> dict =
            new Dictionary<InfoHash, Dictionary<string, SafeFileHandle>>(100);

        internal static void Add(InfoHash infoHash, string file, SafeFileHandle fileHandle)
        {
            if (dict.ContainsKey(infoHash))
                return;

            var handles = new Dictionary<string, SafeFileHandle>(10);

            dict.Add(infoHash, handles);
        }

        internal static bool TryGet(InfoHash infoHash, string file, out SafeFileHandle handle)
        {
            handle = default;

            if (!dict.ContainsKey(infoHash))
                return false;

            var fileHandles = dict[infoHash];

            if (!fileHandles.ContainsKey(file))
                return false;

            handle = fileHandles[file];

            return true;
        }

        internal static void Remove(InfoHash infoHash)
        {
            if (!dict.ContainsKey(infoHash))
                return;

            var fileHandles = dict[infoHash];

            foreach (var handle in fileHandles.Values)
            {
                handle.Close();
            }

            dict.Remove(infoHash);
        }

        internal static void Remove(InfoHash infoHash, string file)
        {
            if (!dict.ContainsKey(infoHash))
                return;

            var fileHandles = dict[infoHash];

            if (!fileHandles.ContainsKey(file))
                return;

            fileHandles[file].Close();
            fileHandles.Remove(file);
        }
    }
}