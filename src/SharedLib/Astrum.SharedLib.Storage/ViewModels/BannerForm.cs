namespace Astrum.News.ViewModels;

public class BannerForm
{
    public Guid Id { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string PictureS3Link { get; set; }
}