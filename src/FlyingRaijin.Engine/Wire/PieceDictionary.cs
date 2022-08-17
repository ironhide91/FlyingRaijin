using System.Collections.Generic;

namespace FlyingRaijin.Engine.Wire
{
    internal class PieceDictionary : IRequestPieceBlock
    {
        internal IEnumerable<KeyValuePair<int, PieceBlock>> Pieces { get { return pairs; } }

        internal PieceDictionary(int pieceLength)
        {
            this.pieceLength = pieceLength;
            pairs = new Dictionary<int, PieceBlock>(100);
        }

        public void Request(int index, out PieceBlock block)
        {
            if (pairs.ContainsKey(index))
            {
                block = pairs[index];
                return;
            }

            block = PieceBlockPool.Pool.Get();
            block.InitializeBuffer(pieceLength);
            block.SetPendingPieceLength(pieceLength);

            pairs.Add(index, block);
            block = pairs[index];
        }

        private readonly Dictionary<int, PieceBlock> pairs;
        private readonly int pieceLength;
    }
}