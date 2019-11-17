namespace FlyingRaijin.Bencode.Ast.Base
{
    public static class NodeTypeCastHelper
    {
        public static T CastOrThrow<T>(this NodeBase node) where T : NodeBase
        {
            return (T)node;
        }
    }
}
