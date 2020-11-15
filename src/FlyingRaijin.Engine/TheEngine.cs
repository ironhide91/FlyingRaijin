using FFlyingRaijin.Engine.ActorModel;
using System;

namespace FlyingRaijin.Engine
{
    public class TheEngine
    {
        public static TheEngine Instance { get { return lazy.Value; } }

        private static readonly Lazy<TheEngine> lazy =
            new Lazy<TheEngine>(() => new TheEngine());

        private TheEngine()
        {

        }

        public void Start()
        {
            EngineActorSystem.Instance.Start();
        }

        public void Stop()
        {
            EngineActorSystem.Instance.Stop();
        }
    }
}