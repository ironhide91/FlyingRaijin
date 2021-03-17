using System;
using System.Collections.Generic;

namespace FlyingRaijin.Engine.Tracker
{
    public class TrackerRequest
    {
        public TrackerRequest(
               byte[] infoHash,
               byte[] peerId,
               string ip,
                  int port,
                 long uploaded,
                 long downloaded,
                 long left,
            EventType eventType,
                  int compact,
                  int trackerId,               
                  int noPeerId,
                  int numWant,
                  int key)
        {
              InfoHash = Array.AsReadOnly(infoHash);
                PeerId = Array.AsReadOnly(peerId);
                    IP = ip;
                  Port = port;
              Uploaded = uploaded;
            Downloaded = downloaded;
                  Left = left;
                 Event = eventType;
               Compact = compact;
             TrackerId = trackerId;               
              NoPeerId = noPeerId;
               NumWant = numWant;
                   Key = key;
        }

        public readonly IList<byte> InfoHash;
        public readonly IList<byte> PeerId;
        public readonly string      IP;
        public readonly int         Port;
        public readonly long        Uploaded;
        public readonly long        Downloaded;
        public readonly long        Left;
        public readonly EventType   Event; 
        public readonly int         Compact;
        public readonly int         TrackerId; 
        public readonly int         NoPeerId;
        public readonly int         NumWant;
        public readonly int         Key;
    }
}