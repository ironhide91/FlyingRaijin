namespace FlyingRaijin.Engine.Wire
{
    enum MessageId : byte
    {
        Choke = 0,
        Unchoke = 1,
        Interested = 2,
        NotInterested = 3,
        Have = 4,
        BitField = 5,
        Request = 6,
        Piece = 7,
        Cancel = 8,
        Port = 9,
        UnKnown= 10
    };
}