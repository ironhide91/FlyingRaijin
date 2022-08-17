using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Engine.Wire
{
    internal static class WireProtocolHelper
    {
        private const int ProtocolIdentifierBegin = 0;
        private const int ProtocolIdentifierEnd = 19;
        private const int InfoHashBegin = 28;
        private const int InfoHashEnd = 47;

        internal static HandshakeResponse IsValidHandshakeResponse(this PipeReader pipeReader, ReadOnlyMemory<byte> handshakeToMatchWith)
        {
            if (!pipeReader.TryRead(out ReadResult result))
                return HandshakeResponse.Pending;

            var seqReader = new SequenceReader<byte>(result.Buffer);

            if (seqReader.Length < 68)
                return HandshakeResponse.Pending;

            var roshandshakeToMatchWith = new ReadOnlySequence<byte>(handshakeToMatchWith);
            var hsReader = new SequenceReader<byte>(roshandshakeToMatchWith);

            long consumed = 0;

            // Protocol Length Prefix(1) + Protcol Length(19) = 20 bytes
            if (SequenceNotEquals(ref hsReader, ref seqReader, ProtocolIdentifierBegin, ProtocolIdentifierEnd))
                return HandshakeResponse.Failed;
            consumed += 20;

            // Reserved
            if (seqReader.IsSequenceIncomplete(8))
                return HandshakeResponse.Failed;
            consumed += 8;

            // Info Hash
            if (SequenceNotEquals(ref hsReader, ref seqReader, InfoHashBegin, InfoHashEnd))
                return HandshakeResponse.Failed;
            consumed += 20;

            // Peer Id
            if (seqReader.IsSequenceIncomplete(20))
                return HandshakeResponse.Failed;
            consumed += 20;

            seqReader.Advance(consumed);

            return HandshakeResponse.Succes;
        }

        internal static ValueTuple<int, MessageId> ReadMessageHeader(this PipeReader pipeReader)
        {
            pipeReader.TryRead(out ReadResult result);
            var seqReader = new SequenceReader<byte>(result.Buffer);

            var pooledBytes = ArrayPool<byte>.Shared.Rent(4);
            Span<byte> span = pooledBytes;
            seqReader.TryPeek(0, out pooledBytes[3]);
            seqReader.TryPeek(1, out pooledBytes[2]);
            seqReader.TryPeek(2, out pooledBytes[1]);
            seqReader.TryPeek(3, out pooledBytes[0]);
            seqReader.TryPeek(4, out byte messageType);

            Span<byte> span4 = span.Slice(0, 4);
            var length = BitConverter.ToInt32(span4);
            ArrayPool<byte>.Shared.Return(pooledBytes);

            seqReader.Advance(5);
            pipeReader.AdvanceTo(seqReader.Position);

            if (messageType >= (byte)MessageId.UnKnown)
                return (length, MessageId.UnKnown);

            return (length, (MessageId)messageType);
        }

        internal static int ReadMessageLength(this PipeReader pipeReader)
        {
            pipeReader.TryRead(out ReadResult result);
            var seqReader = new SequenceReader<byte>(result.Buffer);

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
            pipeReader.AdvanceTo(seqReader.Position);

            return length;
        }

        internal static MessageId ReadMessageId(this PipeReader pipeReader)
        {
            pipeReader.TryRead(out ReadResult result);
            var seqReader = new SequenceReader<byte>(result.Buffer);

            seqReader.TryPeek(4, out byte messageType);

            seqReader.Advance(1);
            pipeReader.AdvanceTo(seqReader.Position);

            if (messageType >= (byte)MessageId.UnKnown)
                return MessageId.UnKnown;

            return (MessageId)messageType;
        }

        internal static bool IsControlMessage(this MessageId id)
        {
            switch (id)
            {
                case MessageId.Choke:
                case MessageId.Unchoke:
                case MessageId.Interested:
                case MessageId.NotInterested:
                case MessageId.Cancel:
                    return true;
                default:
                    return false;
            }
        }

        internal static ControlMessage CreateControlMessage(this MessageId id)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            ControlMessage message = id switch
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            {
                MessageId.Choke => ControlMessage.Choke,
                MessageId.Unchoke => ControlMessage.Unchoke,
                MessageId.Interested => ControlMessage.Interested,
                MessageId.NotInterested => ControlMessage.NotInterested,
                MessageId.Cancel => ControlMessage.Cancel
            };

            return message;
        }

        internal static bool IsUnknownMessage(this MessageId id)
        {
            return id == MessageId.UnKnown;
        }

        internal static bool IsDataMessage(this MessageId id)
        {
            switch (id)
            {
                case MessageId.Have:
                case MessageId.BitField:
                case MessageId.Request:
                case MessageId.Piece:
                case MessageId.Port:
                    return true;
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        internal static ValueTuple<bool, HaveMessage> TryParseHaveMessage(this ref SequenceReader<byte> seqReader)
        {
            bool success = false;
            HaveMessage message = default;

            try
            {
                var pooledBytes = ByteArrayPool.Rent(HaveMessage.MessageLength);
                Span<byte> span = pooledBytes;
                seqReader.TryCopyTo(span);
                span.Reverse();

                var index = BitConverter.ToInt32(span);

                ByteArrayPool.Return(pooledBytes);

                message = HaveMessagePool.Pool.Get();
                message.Index = index;

                seqReader.Advance(HaveMessage.MessageLength);
            }
            catch (Exception e)
            {

            }
            
            return (success, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValueTuple<bool, IMessage> TryParseBitFieldMessage(this ref SequenceReader<byte> seqReader, long length)
        {
            bool success = false;
            BitFieldMessage message = default;

            try
            {
                var pooledBytes = ByteArrayPool.Rent(RequestMessage.MessageLength);
                Span<byte> span = pooledBytes;
                seqReader.TryCopyTo(span);
                span.Reverse();

                var index = BitConverter.ToInt32(span.Slice(0, 4));
                var begin = BitConverter.ToInt32(span.Slice(4, 4));
                var rlength = BitConverter.ToInt32(span.Slice(8, 4));

                message = BitFieldMessagePool.Pool.Get();
                message.Index = index;
                message.Begin = begin;
                message.Length = rlength;

                ByteArrayPool.Return(pooledBytes);

                seqReader.Advance(RequestMessage.MessageLength);
            }
            catch (Exception e)
            {

            }

            return (success, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValueTuple<bool, RequestMessage> TryParseRequestMessage(this ref SequenceReader<byte> seqReader)
        {
            bool success = false;
            RequestMessage message = default;

            try
            {
                var pooledBytes = ByteArrayPool.Rent(RequestMessage.MessageLength);
                Span<byte> span = pooledBytes;
                seqReader.TryCopyTo(span);
                span.Reverse();

                var index = BitConverter.ToInt32(span.Slice(0, 4));
                var begin = BitConverter.ToInt32(span.Slice(4, 4));
                var rlength = BitConverter.ToInt32(span.Slice(8, 4));

                ByteArrayPool.Return(pooledBytes);

                message = RequestMessagePool.Pool.Get();
                message.Index = index;
                message.Begin = begin;
                message.Length = rlength;

                seqReader.Advance(RequestMessage.MessageLength);
            }
            catch (Exception e)
            {

            }

            return (success, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValueTuple<bool, PieceMessage> TryParsePieceMessage(
            this ref SequenceReader<byte> seqReader,
            IRequestPieceBlock pieceBlock,
            long length)
        {
            bool success = false;
            PieceMessage message = default;

            try
            {
                var pooledBytes = ByteArrayPool.Rent(RequestMessage.MessageLength);
                Span<byte> span = pooledBytes;
                seqReader.TryCopyTo(span);
                span.Reverse();

                var index = BitConverter.ToInt32(span.Slice(0, 4));
                var begin = BitConverter.ToInt32(span.Slice(4, 4));

                ByteArrayPool.Return(pooledBytes);

                message = PieceMessagePool.Pool.Get();
                message.Index = index;
                message.Begin = begin;

                pieceBlock.Request(index, out PieceBlock destination);
                destination.Write(message, span.Slice(9));
                seqReader.Advance(RequestMessage.MessageLength);
            }
            catch (Exception e)
            {

            }

            return (success, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValueTuple<bool, PortMessage> TryParsePortMessage(this ref SequenceReader<byte> seqReader)
        {
            bool success = false;
            PortMessage message = default;

            try
            {
                var pooledBytes = ByteArrayPool.Rent(2);
                Span<byte> span = pooledBytes;
                seqReader.TryCopyTo(span);
                span.Reverse();

                var port = BitConverter.ToUInt16(span);

                ByteArrayPool.Return(pooledBytes);

                message = PortMessagePool.Pool.Get();
                message.Port = port;

                seqReader.Advance(PortMessage.MessageLength);
            }
            catch (Exception e)
            {

            }

            return (success, message);
        }

        internal static bool SequenceNotEquals(
            ref SequenceReader<byte> source,
            ref SequenceReader<byte> destination,
            int sourceBegin,
            int sourceEnd)
        {
            if (source.Length < sourceEnd)
                return true;

            if (destination.Length < source.Length)
                return true;

            System.Diagnostics.Debug.WriteLine("SequenceEquals");

            for (int i = sourceBegin; i <= sourceEnd; i++)
            {
                source.TryPeek(i, out byte s);
                destination.TryPeek(i, out byte d);

                System.Diagnostics.Debug.WriteLine($"{s},{d}");

                if (!s.Equals(d))
                    return true;

            }

            return false;
        }

        internal static bool IsSequenceIncomplete(this ref SequenceReader<byte> seqReader, long length)
        {
            return (seqReader.Remaining < length);
        }

        internal static bool HasData(this ref SequenceReader<byte> seqReader)
        {
            return !seqReader.End;
        }

        //if ((messageType == MessageType.BitField) && (((length-1) * 8) != pieceLength))
        //        {
        //            remote.Tell(Close.Instance);
        //            return;
        //        }
    }
}