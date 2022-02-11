using Akka.Actor;

namespace FlyingRaijin.Engine
{
    internal abstract class ActorBuilderBase<T1>
    {
        internal ActorBuilderBase()
        {

        }

        internal abstract Props Build();

        protected T1 Value;

        internal void With(T1 value)
        {
            Value = value;
        }        
    }

    internal abstract class ActorBuilderBase<T1, T2>
    {
        internal ActorBuilderBase()
        {

        }        

        protected T1 Value1;
        protected T2 Value2;

        internal abstract Props Build();

        internal void With(T1 value)
        {
            Value1 = value;
        }

        internal void With(T2 value)
        {
            Value2 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2, T3>
    {
        internal ActorBuilderBase()
        {

        }

        protected T1 Value1;
        protected T2 Value2;
        protected T3 Value3;

        internal abstract Props Build();

        internal void With(T1 value)
        {
            Value1 = value;
        }

        internal void With(T2 value)
        {
            Value2 = value;
        }

        internal void With(T3 value)
        {
            Value3 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2, T3, T4>
    {
        internal ActorBuilderBase()
        {

        }

        protected T1 Value1;
        protected T2 Value2;
        protected T3 Value3;
        protected T4 Value4;

        internal abstract Props Build();

        internal void With(T1 value)
        {
            Value1 = value;
        }

        internal void With(T2 value)
        {
            Value2 = value;
        }

        internal void With(T3 value)
        {
            Value3 = value;
        }

        internal void With(T4 value)
        {
            Value4 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2, T3, T4, T5>
    {
        internal ActorBuilderBase()
        {

        }

        protected T1 Value1;
        protected T2 Value2;
        protected T3 Value3;
        protected T4 Value4;
        protected T5 Value5;

        internal abstract Props Build();

        internal void With(T1 value)
        {
            Value1 = value;
        }

        internal void With(T2 value)
        {
            Value2 = value;
        }

        internal void With(T3 value)
        {
            Value3 = value;
        }

        internal void With(T4 value)
        {
            Value4 = value;
        }

        internal void With(T5 value)
        {
            Value5 = value;
        }
    }
}