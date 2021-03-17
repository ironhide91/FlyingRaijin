using System;
using System.Text;
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

        private readonly StringBuilder strBuilder;

        public TrackerRequestBuilder(string announceUrl)
        {
            strBuilder = new StringBuilder(200);
            strBuilder.Append($"{announceUrl}?");
        }

        public string Build()
        {
            return strBuilder.ToString();
        }

        public TrackerRequestBuilder WithInfoHash(byte[] value)
        {
            strBuilder.Append($"{infoHashName}={HttpUtility.UrlEncode(value)}&");
            return this;
        }

        public TrackerRequestBuilder WithPeerId(string value)
        {
            strBuilder.Append($"{peerIdName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithIP(string value)
        {
            strBuilder.Append($"{ipName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithPort(int value)
        {
            strBuilder.Append($"{portName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithUploaded(long value)
        {
            strBuilder.Append($"{uploadedName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithDownloaded(long value)
        {
            strBuilder.Append($"{downloadedName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithLeft(long value)
        {
            strBuilder.Append($"{leftName}={value}&");
            return this;
        }        

        public TrackerRequestBuilder WithCompact(int value)
        {
            strBuilder.Append($"{compactName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithEvent(EventType value)
        {
            switch (value)
            {
                case EventType.Started:
                    strBuilder.Append($"{eventTypeName}={eventTypeStarted}&");
                    break;
                case EventType.Stopped:
                    strBuilder.Append($"{eventTypeName}={eventTypeStopped}&");
                    break;
                case EventType.Completed:
                    strBuilder.Append($"{eventTypeName}={eventTypeCompleted}&");
                    break;
                default:
                    break;
            }
            
            return this;
        }

        public TrackerRequestBuilder WithTrackerId(int value)
        {
            strBuilder.Append($"{trackerIdName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithNoPeerId(int value)
        {
            strBuilder.Append($"{noPeerIdName}={value}&");
            return this;
        }        

        public TrackerRequestBuilder WithNumWant(int value)
        {
            strBuilder.Append($"{numWantName}={value}&");
            return this;
        }

        public TrackerRequestBuilder WithKey(int value)
        {
            strBuilder.Append($"{keyName}={value}&");
            return this;
        }
    }
}