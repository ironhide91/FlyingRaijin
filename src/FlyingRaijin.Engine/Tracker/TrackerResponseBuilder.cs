using System.Collections.Generic;

namespace FlyingRaijin.Engine.Tracker
{
    public class TrackerResponseBuilder
    {
        private string            reason;
        private string            warning;
        private long              interval;
        private long              minInterval;        
        private long              complete;
        private long              inComplete;
        private string            trackerId;
        private IEnumerable<Peer> peers;

        private TrackerResponseType responseType;

        public TrackerResponseBuilder()
        {

        }

        public TrackerResponse Build()
        {
            var response = new TrackerResponse(
                reason,
                warning,
                interval,
                minInterval,                
                complete,
                inComplete,
                trackerId,
                peers,
                responseType);

            return response;
        }

        public TrackerResponseBuilder WithReason(string value)
        {
            reason = value;
            return this;
        }

        public TrackerResponseBuilder WithWarning(string value)
        {
            warning = value;
            return this;
        }

        public TrackerResponseBuilder WithInterval(long value)
        {
            interval = value;
            return this;
        }

        public TrackerResponseBuilder WithMinInterval(long value)
        {
            minInterval = value;
            return this;
        }        

        public TrackerResponseBuilder WithComplete(long value)
        {
            complete = value;
            return this;
        }

        public TrackerResponseBuilder WithInComplete(long value)
        {
            inComplete = value;
            return this;
        }

        public TrackerResponseBuilder WithTrackerId(string value)
        {
            trackerId = value;
            return this;
        }

        public TrackerResponseBuilder WithPeers(IEnumerable<Peer> value)
        {
            peers = value;
            return this;
        }

        public TrackerResponseBuilder WithResponseType(TrackerResponseType value)
        {
            responseType = value;
            return this;
        }
    }
}