namespace FlyingRaijin.Bencode
{
    public enum Production
    {
        TORRENT,
        //- Dictionary
        BENCODED_DICTIONARY,
        DICTIONARY_START,
        DICTIONARY_ELEMENTS,
        DICTIONARY_KEY_VALUE,
        //- List
        BENCODED_LIST,
        LIST_START,
        LIST_ELEMENTS,
        //- Integer
        BENCODED_INTEGER,
        INTEGER_START,
        NEGATIVE_SIGN,
        INTEGER,
        END,
        //- String
        BENCODED_STRING,
        STRING_LENGTH_PREFIX,
        STRING,
        BYTE,
        //- Shared
        NUMBER,
        DIGIT_EXCULUDING_ZERO,
        ZERO,
        //- Sentinel
        Sentinel
    }
}