using Astrum.News.DomainServices.ViewModels;
using Microsoft.AspNetCore.Http;
namespace Astrum.News.ViewModels;

public class PostForm
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }
    public int? ReadingTime { get; set; }
    public bool IsArticle { get; set; }
    public Guid From { get; set; }
    public UserInfo User { get; set; }
    public int LikesCount { get; set; }
    public Guid? LikeId { get; set; }
    public DateTimeOffset? DateCreated { get; set; }

    //[NotMapped]
    //[DisplayName("Upload File")]
    //public IFormFile ImageFile { get; set; }
    public List<CommentForm> Comments { get; set; } = new();
    public List<LikeForm> Likes { get; set; } = new();
    public List<AttachmentFileForm> Attachments { get; set; } = new();
}