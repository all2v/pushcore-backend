namespace Astrum.News.ViewModels;

public class LikeForm
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid From { get; set; }
    public UserInfo User { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
}