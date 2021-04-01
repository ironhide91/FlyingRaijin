using Akka.Actor;
using FlyingRaijin.Messages;
using System.Net.Http;

namespace FlyingRaijin.Engine.Actors
{
    public class HttpClientActor : ReceiveActor
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public HttpClientActor()
        {
            Receive<HttpGetCommand>(command => OnGetRequest(command));
        }

        private void OnGetRequest(HttpGetCommand command)
        {
            var response = httpClient.GetAsync(command.Url).Result;
            Sender.Tell(response);
        }
    }
}