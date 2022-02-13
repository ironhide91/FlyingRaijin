﻿using Microsoft.Extensions.ObjectPool;

namespace FlyingRaijin.Engine.Wire
{
    internal sealed class PieceBlockPoolPolicy : IPooledObjectPolicy<PieceBlock>
    {
        internal static readonly PieceBlockPoolPolicy Instance = new PieceBlockPoolPolicy();

        public PieceBlock Create()
        {
            return new PieceBlock();
        }

        public bool Return(PieceBlock obj)
        {
            obj.SetPendingPieceLength(int.MinValue);
            obj.ReleaseBuffer();

            return true;
        }
    }
}