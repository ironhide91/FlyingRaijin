using FlyingRaijin.Bencode.BObject;
using Serilog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IdGen;
using Serilog.Context;

namespace FlyingRaijin.Bencode.Read
{
    public static partial class BencodeParser
    {
        private static readonly IdGenerator idGenerator = new IdGenerator(0);

        private static readonly ILogger log = Scroll.Logger.ForContext(typeof(BencodeParser));

        private static readonly ParseResult ErrorResult = new ParseResult(ErrorType.Unknown, null);

        private const string InfoKey = "info";

        private const string PeerId = "peer id";

        public static ParseResult<T> Parse<T>(ReadOnlySpan<byte> bytes) where T : IBObject
        {
            var temp = idGenerator.CreateId();

            log.Debug($"{temp}");

            using (var logContext = LogContext.PushProperty("CorrelationId", idGenerator.CreateId()))
            {
                var result = Parse(bytes);

                if (result.BObject == null || result.BObject is T)
                    return new ParseResult<T>(result.Error, (T)result.BObject);
            }

            return new ParseResult<T>(ErrorType.Unknown, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ParseResult Parse(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
                return ErrorResult;

            IBObject root;

            ErrorType error = ErrorType.None;

            int index = -1;

            int dictionaryCount = 0;

            int listCount = 0;

            var temp = idGenerator.CreateId();

            log.Debug($"MetaInfo length {bytes.Length}");

            // Determine root type
            switch (bytes[++index])
            {
                // Dictionary
                case dictStart:
                    dictionaryCount++;
                    var dict = new Dictionary<BString, IBObject>();
                    var bdict = new BDictionary(null, dict);
                    root = bdict;
                    break;
                // List
                case listStart:
                    listCount++;
                    var list = new List<IBObject>();
                    var bList = new BList(null, list);
                    root = bList;
                    break;
                // Integer
                case intStart:
                    error = ParseSingleInteger(bytes, null, ref index, out root);
                    if (error.HasError())
                        return ErrorResult;
                    return ErrorResult;
                // String
                case byte b when PositiveIntegerBytes.Contains(b):
                    error = ParseSingleString(bytes, null, isByteEncoded: false, ref index, out root);
                    if (error.HasError())
                        return ErrorResult;
                    return ErrorResult;
                // Unknown
                default:
                    return ErrorResult;
            }

            IBObject currentParent = root;

            IBObject key = null;

            bool expectingKey = (dictionaryCount > 0);

             int infoDictionatStartIndex = -1;          
             int infoDictionatEndIndex   = -1;
             int infoDictionatCount      = -1;
            bool isInfoDictionary        = false;

            // Process nested types
            while (++index < bytes.Length)
            {
                switch (bytes[index])
                {
                    // Dictionary
                    case dictStart:
                        dictionaryCount++;
                        if (isInfoDictionary)
                        {
                            infoDictionatCount = dictionaryCount;
                            infoDictionatStartIndex = index;                            
                        }
                        error = ParseDictionary(ref currentParent, key);
                        if (error.HasError())
                            return new ParseResult(error, null);
                        SetExpectingKeyFlag(ref dictionaryCount, ref expectingKey);
                        break;
                    // List
                    case listStart:
                        listCount++;
                        error = ParseList(ref currentParent, key);
                        if (error.HasError())
                            return new ParseResult(error, null);
                        SetExpectingKeyFlag(ref dictionaryCount, ref expectingKey);
                        break;
                    // Integer
                    case intStart:
                        error = ParseInteger(bytes, currentParent, ref index, key);
                        if (error.HasError())
                            return new ParseResult(error, null);
                        SetExpectingKeyFlag(ref dictionaryCount, ref expectingKey);
                        break;
                    // End { Dictionary, List, Integer }
                    case end:
                        if (currentParent is BDictionary)
                        {
                            (currentParent as BDictionary).SyncInternalStringDictionary();
                            if (dictionaryCount == infoDictionatCount)
                            {
                                isInfoDictionary = false;
                                infoDictionatEndIndex = (index);
                            }
                            dictionaryCount--;
                            if (currentParent.Parent != null)
                                currentParent = currentParent.Parent;                            
                            break;
                        }
                        if (currentParent is BList)
                        {
                            listCount--;
                            if (currentParent.Parent != null)
                                currentParent = currentParent.Parent;
                            break;
                        }
                        break;
                    // String
                    case byte b when PositiveIntegerBytes.Contains(b):
                        error = ParseString(bytes, currentParent, ref index, ref expectingKey, ref key);
                        if (error.HasError())
                            return new ParseResult(error, null);
                        if (key is BString @keyString && keyString.StringValue == InfoKey)
                            isInfoDictionary = true;
                        break;
                    // Unkown
                    default:
                        return new ParseResult(ErrorType.Unknown, null);
                }
            }

            if (dictionaryCount != 0)
                return new ParseResult(ErrorType.DictionaryInvalid, null);

            if (listCount != 0)
                return new ParseResult(ErrorType.ListInvalid, null);            

            return new ParseResult(error, currentParent, infoDictionatStartIndex, infoDictionatEndIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetExpectingKeyFlag(ref int dictionaryCount, ref bool expectingKey)
        {
            if (dictionaryCount > 0)
                expectingKey = true;
        }
    }
}