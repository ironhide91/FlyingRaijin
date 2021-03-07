using System;
using System.Collections.Specialized;
using System.Web;

namespace FlyingRaijin.Engine.Tracker
{
    public class TrackerRequestBuilder
    {
        private const string infoHashName   = "info_hash";
        private const string peerIdName     = "peer_id";
        private const string ipName         = "ip";
        private const string portName       = "port";
        private const string uploadedName   = "uploaded";
        private const string downloadedName = "downloaded";
        private const string leftName       = "left";
        private const string eventTypeName  = "event";
        private const string compactName    = "compact";
        private const string trackerIdName  = "trackerid";
        private const string noPeerIdName   = "no_peer_id";
        private const string numWantName    = "numwant";
        private const string keyName        = "key";

        private const string eventTypeStarted   = "started";
        private const string eventTypeStopped   = "stopped";
        private const string eventTypeCompleted = "completed";

        private readonly UriBuilder          uriBuilder;
        private readonly NameValueCollection query;

        public TrackerRequestBuilder(string uri)
        {
            uriBuilder = new UriBuilder(uri);

            query = HttpUtility.ParseQueryString(uriBuilder.Query);
        }

        public string Build()
        {
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        public TrackerRequestBuilder WithInfoHash(byte[] value)
        {
            query[infoHashName] = HttpUtility.UrlEncode(value);
            return this;
        }

        public TrackerRequestBuilder WithPeerId(string value)
        {
            query[peerIdName] = HttpUtility.UrlEncode(value);
            return this;
        }

        public TrackerRequestBuilder WithIP(string value)
        {
            query[ipName] = value;
            return this;
        }

        public TrackerRequestBuilder WithPort(int value)
        {
            query[portName] = value.ToString();
            return this;
        }

        public TrackerRequestBuilder WithUploaded(long value)
        {
            query[uploadedName] = value.ToString();
            return this;
        }

        public TrackerRequestBuilder WithDownloaded(long value)
        {
            query[downloadedName] = value.ToString();
            return this;
        }

        public TrackerRequestBuilder WithLeft(long value)
        {
            query[leftName] = value.ToString();
            return this;
        }        

        public TrackerRequestBuilder WithCompact(int value)
        {
            query[compactName] = value.ToString();
            return this;
        }

        public TrackerRequestBuilder WithEvent(EventType value)
        {
            switch (value)
            {
                case EventType.Started:
                    query[eventTypeName] = eventTypeStarted;
                    break;
                case EventType.Stopped:
                    query[eventTypeName] = eventTypeStopped;
                    break;
                case EventType.Completed:
                    query[eventTypeName] = eventTypeCompleted;
                    break;
                default:
                    break;
            }
            
            return this;
        }

        public TrackerRequestBuilder WithTrackerId(int value)
        {
            query[trackerIdName] = value.ToString();
            return this;
        }

        public TrackerRequestBuilder WithNoPeerId(int value)
        {
            query[noPeerIdName] = value.ToString();
            return this;
        }        

        public TrackerRequestBuilder WithNumWant(int value)
        {
            query[numWantName] = value.ToString();
            return this;
        }

        public TrackerRequestBuilder WithKey(int value)
        {
            query[keyName] = value.ToString();
            return this;
        }
    }
}