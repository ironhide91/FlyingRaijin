using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRaijin.Engine.Tracker
{
    public static class TrackerResponseParser
    {
        public static TrackerResponse Parse(ReadOnlySpan<byte> response)
        {
            var bDict = BencodeParser.Parse<BDictionary>(response);

            if (bDict.Error != ErrorType.None)
                return null;

            if (bDict.BObject == null)
                return null;

            if (bDict.BObject.ContainsKey(TrackerResponse.FailureReasonKey))
                return Failure(bDict);

            if (bDict.BObject.ContainsKey(TrackerResponse.WarningKey))
                return Warning(bDict);

            return Success(bDict);
        }

        private static TrackerResponse Failure(ParseResult<BDictionary> bDict)
        {
            var responseBuilder = new TrackerResponseBuilder();

            responseBuilder
                .WithReason(bDict.GetFailureReason())
                .WithResponseType(TrackerResponseType.Failure);                

            return responseBuilder.Build();
        }

        private static TrackerResponse Warning(ParseResult<BDictionary> bDict)
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

        private static TrackerResponse Success(ParseResult<BDictionary> bDict)
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

        private static string GetFailureReason(this ParseResult<BDictionary> bDict)
        {
            BString result;

            if (bDict.BObject.TryGetValue<BString>(TrackerResponse.FailureReasonKey, out result))
            {
                return result.StringValue;
            }

            return string.Empty;
        }

        private static string GetWarningReason(this ParseResult<BDictionary> bDict)
        {
            BString result;

            if (bDict.BObject.TryGetValue(TrackerResponse.WarningKey, out result))
            {
                return result.StringValue;
            }

            return string.Empty;
        }

        private static long GetInterval(this ParseResult<BDictionary> bDict)
        {
            BInteger result;

            if (bDict.BObject.TryGetValue(TrackerResponse.IntervalKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static long GetMinInterval(this ParseResult<BDictionary> bDict)
        {
            BInteger result;

            if (bDict.BObject.TryGetValue(TrackerResponse.MinIntervalKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static long GetComplete(this ParseResult<BDictionary> bDict)
        {
            BInteger result;

            if (bDict.BObject.TryGetValue(TrackerResponse.CompleteKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static long GetInComplete(this ParseResult<BDictionary> bDict)
        {
            BInteger result;

            if (bDict.BObject.TryGetValue(TrackerResponse.InCompleteKey, out result))
            {
                return result.Value;
            }

            return 0L;
        }

        private static string GetTrackerId(this ParseResult<BDictionary> bDict)
        {
            BString result;

            if (bDict.BObject.TryGetValue(TrackerResponse.TrackerIdKey, out result))
            {
                return result.StringValue;
            }

            return string.Empty;
        }

        private static IEnumerable<Peer> GetPeers(this ParseResult<BDictionary> bDict)
        {
            BString result;

            if (bDict.BObject.TryGetValue(TrackerResponse.PeersKey, out result))
            {
                IList<Peer> peers;
                CompactPeerParser.TryParsePeers(result.Value.Span, out peers);
                return peers;
            }

            return Enumerable.Empty<Peer>();
        }
    }
}