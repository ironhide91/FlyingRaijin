using Akka.Actor;
using Akka.Configuration;
using FlyingRaijin.Controller.Actors;
using FlyingRaijin.Messages;
using System;

namespace FlyingRaijin.Controller
{
    public class TheController : IDisposable
    {
        private TheController()
        {
            var config = ConfigurationFactory.ParseString(@"
                akka {  
                    actor {
                        provider = remote
                    }
                    remote {
                        dot-netty.tcp {
		                    port = 0
		                    hostname = localhost
                        }
                    }
                }
            ");

            clientActorSystem = ActorSystem.Create("Client", config);

            newTorrentClientActorRef = clientActorSystem.ActorOf<NewTorrentClientActor>();
        }

        public static TheController Instance { get { return lazy.Value; } }

        private static Lazy<TheController> lazy = new Lazy<TheController>(() => new TheController());
        private readonly ActorSystem clientActorSystem;
        private readonly IActorRef newTorrentClientActorRef;

        public void Add(string filePath)
        {
            System.Diagnostics.Debug.WriteLine("");
            foreach (var item in System.Text.Encoding.UTF8.GetBytes(filePath))
            {
                System.Diagnostics.Debug.Write(item);
            }
            System.Diagnostics.Debug.WriteLine("");

            newTorrentClientActorRef.Tell(new NewTorrentCommand(filePath));
        }

        public void Dispose()
        {
            clientActorSystem?.Dispose();
        }
    }
}