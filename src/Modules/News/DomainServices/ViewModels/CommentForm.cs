namespace Astrum.News.ViewModels;

public class CommentForm
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid From { get; set; }
    public Guid? ReplyCommentId { get; set; }
    public List<CommentForm> ChildComments { get; set; }
    public UserInfo User { get; set; }
    public string Text { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
}