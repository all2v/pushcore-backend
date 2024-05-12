using Astrum.SharedLib.Persistence.Models;

namespace Astrum.News.Entities;

public class BannerEntity : DataEntityBase<Guid>
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string PictureS3Link { get; set; }
    public Guid From { get; set; }
    public DateTimeOffset? Created { get; set; }
}