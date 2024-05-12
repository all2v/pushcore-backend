namespace Astrum.Storage.ViewModels;

public class FileForm
{
    public Guid Id { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string HashSum { get; set; }
    public Stream? FileStream { get; set; }
    public byte[]? FileBytes { get; set; }
}