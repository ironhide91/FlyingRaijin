using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Torrent;
using System;
using System.Collections.Generic;

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

            if (bDict.BObject.ContainsKey(TrackerResponse.WarningKey))
                return Success(bDict);

            return null;
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
                .WithInterval(bDict.GetMinInterval())
                .WithMinInterval(bDict.GetInterval())
                .WithComplete(bDict.GetMinInterval())
                .WithInComplete(bDict.GetInterval())
                .WithTrackerId(bDict.GetTrackerId())
                .WithPeers(bDict.GetPeers());

            return responseBuilder.Build();
        }

        private static TrackerResponse Success(ParseResult<BDictionary> bDict)
        {
            var responseBuilder = new TrackerResponseBuilder()
                .WithResponseType(TrackerResponseType.Success)
                .WithInterval(bDict.GetMinInterval())
                .WithMinInterval(bDict.GetInterval())
                .WithComplete(bDict.GetMinInterval())
                .WithInComplete(bDict.GetInterval())
                .WithTrackerId(bDict.GetTrackerId())
                .WithPeers(bDict.GetPeers());

            return responseBuilder.Build();
        }

        private static string GetFailureReason(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BString>(TrackerResponse.FailureReasonKey);

            return bValue.StringValue;
        }

        private static string GetWarningReason(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BString>(TrackerResponse.WarningKey);

            return bValue.StringValue;
        }

        private static long GetInterval(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BInteger>(TrackerResponse.IntervalKey);

            return bValue.Value;
        }

        private static long GetMinInterval(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BInteger>(TrackerResponse.MinIntervalKey);

            return bValue.Value;
        }

        private static long GetComplete(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BInteger>(TrackerResponse.CompleteKey);

            return bValue.Value;
        }

        private static long GetInComplete(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BInteger>(TrackerResponse.InCompleteKey);

            return bValue.Value;
        }

        private static string GetTrackerId(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BString>(TrackerResponse.TrackerIdKey);

            return bValue.StringValue;
        }

        private static IEnumerable<Peer> GetPeers(this ParseResult<BDictionary> bDict)
        {
            var bValue = bDict.BObject.GetValue<BString>(TrackerResponse.TrackerIdKey);

            return null;
        }
    }
}