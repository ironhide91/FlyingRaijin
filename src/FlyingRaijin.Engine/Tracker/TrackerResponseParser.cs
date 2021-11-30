using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FlyingRaijin.Engine.Tracker
{
    public static class TrackerResponseParser
    {
        public static bool TryParse(ReadOnlySpan<byte> response, out TrackerResponse output)
        {
            var result = BencodeParser.Parse<BDictionary>(response);

            if (result.Error != ErrorType.None)
            {
                output = null;
                return false;
            }

            if (result == null || result.BObject == null)
            {
                output = null;
                return false;
            }

            if (result.BObject.ContainsKey(TrackerResponse.FailureReasonKey))
            {
                output = Failure(result.BObject);
                return true;
            }

            if (result.BObject.ContainsKey(TrackerResponse.WarningKey))
            {
                output = Warning(result.BObject);
                return true;
            }

            output = Success(result.BObject);
            return true;
        }

        public static TrackerResponse Failure(BDictionary bDict)
        {
            var responseBuilder = new TrackerResponseBuilder();

            responseBuilder
                .WithReason(bDict.GetFailureReason())
                .WithResponseType(TrackerResponseType.Failure);                

            return responseBuilder.Build();
        }

        public static TrackerResponse Warning(BDictionary bDict)
        {
            var responseBuilder = new TrackerResponseBuilder()
                .WithReason(bDict.GetWarningReason())
                .WithResponseType(TrackerResponseType.Warning)
                .WithInterval(bDict.GetInterval())
                .WithMinInterval(bDict.GetMinInterval())
                .WithComplete(bDict.GetComplete())
                .WithInComplete(bDict.GetInComplete())
                .WithTrackerId(bDict.GetTrackerId())
                .WithPeers(bDict.GetPeers());

            return responseBuilder.Build();
        }

        public static TrackerResponse Success(BDictionary bDict)
        {
            var responseBuilder = new TrackerResponseBuilder()
                .WithResponseType(TrackerResponseType.Success)
                .WithInterval(bDict.GetInterval())
                .WithMinInterval(bDict.GetMinInterval())
                .WithComplete(bDict.GetComplete())
                .WithInComplete(bDict.GetInComplete())
                .WithTrackerId(bDict.GetTrackerId())
                .WithPeers(bDict.GetPeers());

            return responseBuilder.Build();
        }

        public static string GetFailureReason(this BDictionary bDict)
        {
            BString result;

            if (bDict.TryGetValue(TrackerResponse.FailureReasonKey, out result))
            {
                return result.StringValue;
            }

            return string.Empty;
        }

        public static string GetWarningReason(this BDictionary bDict)
        {
            BString result;

            if (bDict.TryGetValue(TrackerResponse.WarningKey, out result))
            {
                return result.StringValue;
            }

            return string.Empty;
        }

        public static long GetInterval(this BDictionary bDict)
        {
            BInteger result;

            if (bDict.TryGetValue(TrackerResponse.IntervalKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        public static long GetMinInterval(this BDictionary bDict)
        {
            BInteger result;

            if (bDict.TryGetValue(TrackerResponse.MinIntervalKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static long GetComplete(this BDictionary bDict)
        {
            BInteger result;

            if (bDict.TryGetValue(TrackerResponse.CompleteKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static long GetInComplete(this BDictionary bDict)
        {
            BInteger result;

            if (bDict.TryGetValue(TrackerResponse.InCompleteKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static string GetTrackerId(this BDictionary bDict)
        {
            BString result;

            if (bDict.TryGetValue(TrackerResponse.TrackerIdKey, out result))
            {
                return result.StringValue;
            }

            return string.Empty;
        }

        private static IEnumerable<DnsEndPoint> GetPeers(this BDictionary bDict)
        {
            BString result;

            if (bDict.TryGetValue(TrackerResponse.PeersKey, out result))
            {
                IList<DnsEndPoint> peers;
                CompactPeerParser.TryParsePeers(result.Value.Span, out peers);
                return peers;
            }

            return Enumerable.Empty<DnsEndPoint>();
        }
    }
}