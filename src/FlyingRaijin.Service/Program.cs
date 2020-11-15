using System;
using Topshelf;
using FlyingRaijin.Engine;

namespace FlyingRaijin.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfig =>
            {
                hostConfig.Service<TheEngine>(serviceConfig =>
                {
                    serviceConfig.ConstructUsing(() => TheEngine.Instance);
                    serviceConfig.WhenStarted(engine => engine.Start());
                    serviceConfig.WhenStopped(engine => engine.Stop());
                });
            });
        }
    }
} 