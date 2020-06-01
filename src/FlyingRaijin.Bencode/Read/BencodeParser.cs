using FlyingRaijin.Bencode.BObject;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace FlyingRaijin.Bencode.Read
{

    public static partial class BencodeParser
    {
        public static ParseResult Parse(ReadOnlySpan<byte> bytes)
        {
            IBObject root = null;

            ErrorType error = ErrorType.None;

            int index = -1;

            int dictionaryCount = 0;

            int listCount = 0;

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
                    error = ParseSingleInteger(bytes, ref index, null, out root);
                    if (error.HasError())
                        return new ParseResult(error, null);
                    return new ParseResult(error, root);
                // String
                case byte b when PositiveIntegerBytes.Contains(b):
                    error = ParseSingleString(bytes, ref index, null, out root);
                    if (error.HasError())
                        return new ParseResult(error, null);
                    return new ParseResult(error, root);
                // Unkown
                default:
                    return new ParseResult(ErrorType.Unknown, null);
            }            

            IBObject currentParent = root;

            IBObject key = null;

            bool expectingKey = (dictionaryCount > 0);

            // Process nested types
            while (++index < bytes.Length)
            {
                switch (bytes[index])
                {
                    // Dictionary
                    case dictStart:
                        dictionaryCount++;
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
                        error = ParseInteger(bytes, ref index, currentParent, key);
                        if (error.HasError())
                            return new ParseResult(error, null);
                        SetExpectingKeyFlag(ref dictionaryCount, ref expectingKey);
                        break;
                    // End { Dictionary, List, Integer }
                    case end:
                        if (currentParent is BDictionary)
                        {
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
                        error = ParseString(bytes, ref index, currentParent, ref expectingKey, ref key);
                        if (error.HasError())
                            return new ParseResult(error, null);
                        break;
                    // Unkown
                    default:
                        return new ParseResult(ErrorType.Unknown, null);
                }
            }

            return new ParseResult(error, currentParent);
        }

        private static void SetExpectingKeyFlag(ref int dictionaryCount, ref bool expectingKey)
        {
            if (dictionaryCount > 0)
                expectingKey = true;
        }
    }
}