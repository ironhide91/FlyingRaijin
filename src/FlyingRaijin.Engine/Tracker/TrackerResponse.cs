using System.Collections.Generic;
using System.Net;

namespace FlyingRaijin.Engine.Tracker
{
    public class TrackerResponse
    {
        public const string FailureReasonKey = "failure reason";
        public const string       WarningKey = "warning message";
        public const string      IntervalKey = "interval";
        public const string   MinIntervalKey = "min interval";
        public const string     TrackerIdKey = "tracker id";
        public const string      CompleteKey = "complete";
        public const string    InCompleteKey = "incomplete";
        public const string         PeersKey = "peers";

        public readonly string                   Reason;
        public readonly string                   Warning;
        public readonly long                     Interval;
        public readonly long                     MinInterval;        
        public readonly long                     Complete;
        public readonly long                     InComplete;
        public readonly string                   TrackerId;
        public readonly IEnumerable<DnsEndPoint> Peers;

        public readonly TrackerResponseType ResponseType;

        public TrackerResponse(
                         string reason,
                         string warning,
                           long interval,
                           long minInterval,
                           long complete,
                           long inComplete,
                         string trackerId,
              IEnumerable<DnsEndPoint> peers,
            TrackerResponseType responseType)
        {
                   Reason = reason;
                  Warning = warning;
                 Interval = interval;
              MinInterval = minInterval;                
                 Complete = complete;
               InComplete = inComplete;
                TrackerId = trackerId;
                    Peers = peers;
             ResponseType = responseType;
        }
    }
}