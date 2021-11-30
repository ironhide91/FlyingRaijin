namespace FlyingRaijin.Engine.Wire
{
    enum MessageType : byte
    {
        Choke,
        UnChoke,
        Interested,
        NotInterested,
        Have,
        BitField,
        Request,
        Piece,
        Cancel,
        UnKnown
    };
}