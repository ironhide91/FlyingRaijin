using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace FlyingRaijin.Engine.Wire
{
    internal static class ProtocolHelper
    {
        private const int ProtocolIdentifierBegin = 0;
        private const int   ProtocolIdentifierEnd = 19;
        private const int           ReservedBegin = 20;
        private const int             ReservedEnd = 27;
        private const int           InfoHashBegin = 28;
        private const int             InfoHashEnd = 47;

        internal static async Task Push(PipeWriter writer, byte[] data)
        {
            writer.Write(data);
            writer.Advance(data.Length);
            _ = await writer.FlushAsync();
        }

        internal static bool IsValidHandshakeResponse(
            ref SequenceReader<byte> hsReader,
            ref SequenceReader<byte> seqReader)
        {
            long consumed = 0;

            // Protocol Length Prefix(1) + Protcol Length(19) = 20 bytes
            var failed = !SequenceEquals(hsReader, ProtocolIdentifierBegin, ProtocolIdentifierEnd, seqReader);
            if (failed)
                return false;
            consumed += 20;

            // Reserved
            failed = !SequenceHasEnough(seqReader, 8);
            if (failed)
                return false;
            consumed += 8;

            // Info Hash
            failed = !SequenceEquals(hsReader, InfoHashBegin, InfoHashEnd, seqReader);
            if (failed)
                return false;
            consumed += 20;

            // Peer Id
            failed = !SequenceHasEnough(seqReader, 20);
            if (failed)
                return false;
            consumed += 20;

            seqReader.Advance(consumed);            

            return true;
        }

        internal static int TryReadMessageLength(ref SequenceReader<byte> seqReader)
        {
            if (!SequenceHasEnough(seqReader, 4))
                return int.MinValue;

            var pooledBytes = ArrayPool<byte>.Shared.Rent(4);

            Span<byte> span = pooledBytes;
            
            seqReader.TryPeek(0, out pooledBytes[3]);
            seqReader.TryPeek(1, out pooledBytes[2]);
            seqReader.TryPeek(2, out pooledBytes[1]);
            seqReader.TryPeek(3, out pooledBytes[0]);

            Span<byte> span4 = span.Slice(0, 4);

            var length = BitConverter.ToInt32(span4);

            ArrayPool<byte>.Shared.Return(pooledBytes);

            seqReader.Advance(4);

            System.Diagnostics.Debug.WriteLine($"length {length}");            

            return length;
        }

        internal static MessageType TryReadMessageType(ref SequenceReader<byte> seqReader)
        {
            if (!SequenceHasEnough(seqReader, 1))
                return byte.MinValue;            

            seqReader.TryPeek(0, out byte messageType);

            seqReader.Advance(1);

            if (messageType >= (byte)MessageType.UnKnown)
                return MessageType.UnKnown;

            return (MessageType)messageType;
        }

        public static void TryReadMessage(byte messageType, SequenceReader<byte> seqReader)
        {
            switch (messageType)
            {
                case 0:
                    //choke
                    break;
                case 1:
                    //unchoke
                    break;
                case 2:
                    //interested
                    break;
                case 3:
                    //not interested
                    break;
                case 4:
                    //have
                    break;
                case 5:
                    // BitField
                    break;
                case 6:
                    //request
                    break;
                case 7:
                    //piece
                    break;
                case 8:
                    //cancel
                    break;
                default:
                    break;
            }
        }

        private static void TryReadBitFieldMessage(SequenceReader<byte> seqReader)
        {

        }

        private static bool SequenceEquals(
            SequenceReader<byte> source,
                             int sourceBegin,
                             int sourceEnd,
            SequenceReader<byte> destination)
        {
            if (source.Length < sourceEnd)
                return false;

            if (destination.Length < source.Length)
                return false;

            System.Diagnostics.Debug.WriteLine("SequenceEquals");

            for (int i = sourceBegin; i <= sourceEnd; i++)
            {
                     source.TryPeek(i, out byte s);
                destination.TryPeek(i, out byte d);

                System.Diagnostics.Debug.WriteLine($"{s},{d}");

                if (!s.Equals(d))
                    return false;

            }

            return true;
        }

        private static bool SequenceHasEnough(SequenceReader<byte> seqReader, int length)
        {
            return (seqReader.UnreadSpan.Length >= length);
        }
    }
}