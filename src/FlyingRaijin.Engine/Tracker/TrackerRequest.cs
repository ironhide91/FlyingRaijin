using System;
using System.Collections.Generic;

namespace FlyingRaijin.Engine.Tracker
{
    internal class TrackerRequest
    {
        internal TrackerRequest Create(
            byte[] infoHash,
            byte[] peerId,
               int port,
              long uploaded,
              long downloaded,
              long left,
               int eventType,
            string ip,
               int NumWant)
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
                NumWant);
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
               int numWant)
        {
              InfoHash = Array.AsReadOnly(infoHash);
                PeerId = Array.AsReadOnly(peerId);
                  Port = port;
              Uploaded = uploaded;
            Downloaded = downloaded;
                  Left = left;
                 Event = eventType;
                    IP = ip;
               NumWant = numWant;
        }

        internal readonly IList<byte> InfoHash;
        internal readonly IList<byte> PeerId;
        internal readonly         int Port;
        internal readonly        long Uploaded;
        internal readonly        long Downloaded;
        internal readonly        long Left;
        internal readonly         int Event;
        internal readonly      string IP;
        internal readonly         int NumWant;
    }
}