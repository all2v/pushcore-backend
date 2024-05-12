using Ardalis.Specification;
using Astrum.News.Aggregates;

namespace Astrum.News.Specifications;

public class GetPostsSpec : Specification<Post>
{
    public static int LikesFetchCount = 5;

    public GetPostsSpec()
    {
        Query
            .OrderByDescending(post => post.DateCreated)
            .Include(x => x.FileAttachments)
            .Include(x=>x.Likes.Take(LikesFetchCount))
            .Include(x=>x.Comments.Where(c => c.ReplyCommentId == null)
                    .OrderByDescending(p => p.DateCreated))
                .ThenInclude(f => f.ChildComments.OrderByDescending(p => p.DateCreated));
    }
}

public class GetPostByIdSpec : GetPostsSpec
{
    public GetPostByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetPostsByUserIdSpec : GetPostsSpec
{
    public GetPostsByUserIdSpec(Guid userId) 
    {
        Query
            .Where(post => post.CreatedBy == userId.ToString());
    }
}

public class GetPostsByUserIdPagination : GetPostsByUserIdSpec
{
    public GetPostsByUserIdPagination(Guid userId, int? startIndex = null, 
        int? count = null) : base(userId)
    {
        if (startIndex.HasValue && startIndex.Value >= 0)
            Query.Skip(startIndex.Value);
        if (count.HasValue && count.Value >= 0)
            Query.Take(count.Value);
    }
}