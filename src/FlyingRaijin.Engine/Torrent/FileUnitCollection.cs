using System.Collections.Immutable;
using System.Linq;

namespace FlyingRaijin.Engine.Torrent
{
    internal class FileUnitCollection
    {
        internal static readonly FileUnitCollection Empty =
            new FileUnitCollection(ImmutableList.CreateRange(Enumerable.Empty<FileUnit>()));

        internal readonly IImmutableList<FileUnit> Collection;

        internal FileUnit this[int index]
        {
            get
            {
                return Collection[index];
            }
        }

        internal FileUnitCollection(IImmutableList<FileUnit> files)
        {
            Collection = files;
        }     
    }
}