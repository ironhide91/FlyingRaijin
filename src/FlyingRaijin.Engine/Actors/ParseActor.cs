using Akka.Actor;
using FlyingRaijin.Bencode.BObject;
using FlyingRaijin.Bencode.Read;
using FlyingRaijin.Engine.Bencode;
using FlyingRaijin.Engine.Messages;

namespace FlyingRaijin.Engine.Actors
{
    internal class ParseActor : ReceiveActor
    {
        internal ParseActor()
        {
            Receive<FileRead>(command => OnFileReadCommand(command));
        }

        private void OnFileReadCommand(FileRead file)
        {
            var result = BencodeEngine.Parse(file.Buffer);

            switch (result)
            {
                case ParseResult<BDictionary> torrent when result.Error == ErrorType.None:
                    var metaData = BencodeEngine.ReadMetaData(file.Buffer, torrent);
                    Sender.Tell(metaData);
                    break;
                default:
                    // error
                    break;
            }


            file.ReleaseBuffer();
        }
    }
}