using FlyingRaijin.Engine.Torrent;
using Microsoft.Win32.SafeHandles;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FlyingRaijin.Engine
{
    internal static class FileManager
    {
        internal static bool TryAdd(InfoHash infoHash, string file, SafeFileHandle fileHandle)
        {
            if (dict.ContainsKey(infoHash))
                return false;

            var handles = new Dictionary<string, SafeFileHandle>();

            dict.GetOrAdd(infoHash, handles);

            return true;
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

        internal static void CloseInfoHash(InfoHash infoHash)
        {
            if (!dict.ContainsKey(infoHash))
                return;

            var fileHandles = dict[infoHash];

            foreach (var handle in fileHandles.Values)
            {
                handle.Close();
            }

            //dict.Remove(infoHash);
        }

        internal static void CloseFile(InfoHash infoHash, string file)
        {
            if (!dict.ContainsKey(infoHash))
                return;

            var fileHandles = dict[infoHash];

            if (!fileHandles.ContainsKey(file))
                return;

            fileHandles[file].Close();
            fileHandles.Remove(file);
        }

        internal static void CloseAll()
        {
            foreach (var infoHash in dict)
            {
                foreach (var fileHandle in infoHash.Value)
                {
                    fileHandle.Value.Close();
                }
            }
        }

        private static readonly SafeFileHandleDictionary dict = new SafeFileHandleDictionary(100, 100);

        class SafeFileHandleDictionary : ConcurrentDictionary<InfoHash, Dictionary<string, SafeFileHandle>>
        {
            public SafeFileHandleDictionary()
            {

            }

            public SafeFileHandleDictionary(int level, int capacity) : base(level, capacity)
            {

            }
        }
    }    
}