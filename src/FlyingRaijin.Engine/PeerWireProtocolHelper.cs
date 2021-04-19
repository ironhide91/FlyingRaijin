using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRaijin.Engine
{
    internal static class PeerWireProtocolHelper
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

        internal static bool IsValidHandshakeResponse(PipeReader reader, ReadOnlySequence<byte> handshake, ReadOnlySequence<byte> buffer)
        {
            if (buffer.IsEmpty)
                return false;

            var seqReader = new SequenceReader<byte>(buffer);

            var hsReader = new SequenceReader<byte>(handshake);

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
            consumed += 28;

            // Peer Id
            failed = !SequenceHasEnough(seqReader, 20);
            if (failed)
                return false;
            consumed += 20;

            seqReader.Advance(consumed);
            reader.AdvanceTo(seqReader.Position);

            return true;
        }

        internal static void TryReadMessage(PipeReader reader, ReadOnlySequence<byte> buffer)
        {
            if (buffer.IsEmpty)
                return;

            var seqReader = new SequenceReader<byte>(buffer);            

            if (!SequenceHasEnough(seqReader, 4))
                return;

            var pooledBytes = ArrayPool<byte>.Shared.Rent(4);

            Span<byte> span = pooledBytes;

            while (!seqReader.End)
            {
                seqReader.TryPeek(0, out pooledBytes[3]);
                seqReader.TryPeek(1, out pooledBytes[2]);
                seqReader.TryPeek(2, out pooledBytes[1]);
                seqReader.TryPeek(3, out pooledBytes[0]);

                Span<byte> span4 = span.Slice(0, 4);

                var length = BitConverter.ToInt32(span4);                

                if (length == 0)
                {
                    seqReader.Advance(4);
                    continue;
                }

                seqReader.Advance(4);

                System.Diagnostics.Debug.WriteLine($"length {length}");

                seqReader.TryPeek(0, out byte messageType);

                System.Diagnostics.Debug.WriteLine($"messageType {(decimal)messageType}");
            }

            ArrayPool<byte>.Shared.Return(pooledBytes);
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