using Astrum.SharedLib.Persistence.Models;

namespace Astrum.Storage.Entities;

public class FileEntity : DataEntityBase<Guid>
{
    public string OriginalName { get; set; }
    public string BucketName { get; set; }
    public string ContentType { get; set; }
    public string HashSum { get; set; }
}