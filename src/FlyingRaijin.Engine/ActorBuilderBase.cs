using Akka.Actor;

namespace FlyingRaijin.Engine
{
    internal abstract class ActorBuilderBase
    {
        protected Props ctor;

        internal Props Build()
        {
            return ctor;
        }

        internal IActorRef Build(ActorSystem system)
        {
            return system.ActorOf(ctor);
        }

        internal IActorRef Build(IUntypedActorContext context)
        {
            return context.ActorOf(ctor);
        }
    }

    internal abstract class ActorBuilderBase<T1> : ActorBuilderBase
    {
        internal ActorBuilderBase()
        {

        }

        protected T1 Value1;

        internal void With(T1 value)
        {
            Value1 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2> : ActorBuilderBase<T1>
    {
        internal ActorBuilderBase()
        {

        }

        protected T2 Value2;

        internal void With(T2 value)
        {
            Value2 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2, T3> : ActorBuilderBase<T1, T2>
    {
        internal ActorBuilderBase()
        {

        }

        protected T3 Value3;

        internal void With(T3 value)
        {
            Value3 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2, T3, T4> : ActorBuilderBase<T1, T2, T3>
    {
        internal ActorBuilderBase()
        {
            
        }

        protected T4 Value4;

        internal void With(T4 value)
        {
            Value4 = value;
        }
    }

    internal abstract class ActorBuilderBase<T1, T2, T3, T4, T5> : ActorBuilderBase<T1, T2, T3, T4>
    {
        internal ActorBuilderBase()
        {

        }

        protected T5 Value5;

        internal void With(T5 value)
        {
            Value5 = value;
        }
    }
}