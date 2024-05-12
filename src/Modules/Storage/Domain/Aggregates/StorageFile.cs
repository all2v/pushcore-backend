using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Storage.Aggregates;

public class StorageFile : AggregateRootBase<Guid>
{
    public StorageFile() { }

    public StorageFile(Guid id)
    {
        Id = id;
    }

    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string HashSum { get; set; }
}