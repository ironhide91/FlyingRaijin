using FlyingRaijin.Bencode.Read.Ast.Base;
using FlyingRaijin.Bencode.Read.Ast.String;
using System;

namespace FlyingRaijin.Bencode.Read.Parser
{
    public static partial class DelegateParsers
    {
        public static void StringParser(ParserContext context, NodeBase ast, ref int bytesToProcess)
        {
            var pooledBuffer = BytePool.Pool.Rent(bytesToProcess);

            StringNode node = new StringNode(pooledBuffer);

            try
            {
                context.Advance(bytesToProcess, pooledBuffer);
            }
            catch (Exception e)
            {
                BytePool.Pool.Return(pooledBuffer);
                throw e;
            }                        

            ast.Children.Add(node);
        }
    }
}