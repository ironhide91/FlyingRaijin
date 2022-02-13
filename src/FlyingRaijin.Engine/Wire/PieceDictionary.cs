using System.Collections.Generic;

namespace FlyingRaijin.Engine.Wire
{
    internal class PieceDictionary
    {
        public PieceDictionary(int pieceLength)
        {
            this.pieceLength = pieceLength;
            pairs = new Dictionary<int, PieceBlock>(100);
        }

        private readonly Dictionary<int, PieceBlock> pairs;
        private readonly int pieceLength;

        internal IEnumerable<KeyValuePair<int, PieceBlock>> Pieces { get { return pairs; } }

        internal void Add(PieceMessage message)
        {
            if (pairs.ContainsKey(message.Index))
            {
                pairs[message.Index].Add(message);
                return;
            }

            var blocks = PieceBlockPool.Pool.Get();
            blocks.SetPendingPieceLength(pieceLength);

            pairs.Add(message.Index, blocks);
        }
    }
}