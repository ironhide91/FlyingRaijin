using Akka.Actor;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Messages;

namespace FlyingRaijin.Engine.Actors
{
    public class ParseActor : ReceiveActor
    {
        public ParseActor()
        {
            Receive<FileRead>(command => OnFileReadCommand(command));
        }

        private void OnFileReadCommand(FileRead command)
        {
            var result = BencodeEngine.Parse(command.Data.Span);

            switch (result)
            {
                case ParseResult<BDictionary> torrent when result.Error == ErrorType.None:
                    var metaData = BencodeEngine.ReadMetaData(command.Data.Span, torrent);
                    Sender.Tell(metaData);
                    break;
                default:
                    // error
                    break;
            }
        }
    }
}