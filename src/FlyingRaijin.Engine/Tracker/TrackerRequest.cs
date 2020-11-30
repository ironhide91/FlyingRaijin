using System;
using System.Collections.Generic;

namespace FlyingRaijin.Engine.Tracker
{
    internal class TrackerRequest
    {
        internal static TrackerRequest Create(
            byte[] infoHash,
            byte[] peerId,
               int port,
              long uploaded,
              long downloaded,
              long left,
               int eventType,
            string ip,
               int compact)
        {
            return new TrackerRequest(
                infoHash,
                peerId,
                port,
                uploaded,
                downloaded,
                left,
                eventType,
                ip,
                compact);
        }

        private TrackerRequest(
            byte[] infoHash,
            byte[] peerId,
               int port,
              long uploaded,
              long downloaded,
              long left,
               int eventType,
            string ip,
               int compact)
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
        }

        internal readonly IList<byte> InfoHash;
        internal readonly IList<byte> PeerId;        
        internal readonly         int Port;
        internal readonly        long Uploaded;
        internal readonly        long Downloaded;
        internal readonly        long Left;        
        internal readonly         int Compact;
        internal readonly         int NoPeerId;
        internal readonly         int Event;
        // Optional
        internal readonly      string IP;
        internal readonly         int NumWant;
        internal readonly         int Key;
        internal readonly         int TrackerId;
    }
}