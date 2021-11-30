using System.Collections.Immutable;
using System.Linq;

namespace FlyingRaijin.Engine.Torrent
{
    public class FileUnitCollection
    {
        public static readonly FileUnitCollection Empty =
            new FileUnitCollection(ImmutableList.CreateRange(Enumerable.Empty<FileUnit>()));

        public readonly IImmutableList<FileUnit> Collection;

        public FileUnit this[int index]
        {
            get
            {
                return Collection[index];
            }
        }

        public FileUnitCollection(IImmutableList<FileUnit> files)
        {
            Collection = files;
        }     
    }
}