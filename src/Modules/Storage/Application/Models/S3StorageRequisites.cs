namespace Astrum.Storage.Models;

public class S3StorageRequisites
{
    public const string Section = "S3Storage";
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Endpoint { get; set; }
    public string Link { get; set; }
    public string Bucket { get; set; }
    public bool Secure { get; set; }
}
