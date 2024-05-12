using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.TrackerProject.Domain.Aggregates;

namespace Astrum.TrackerProject.Domain.Specification
{
    public class GetArticleByIdSpecification : Specification<Article>
    {
        public GetArticleByIdSpecification(string id)
        {
            Query
                .Where(x => x.Id == id)
                .Include(x => x.Attachments)
                .Include(x => x.Comments);
        }
    }

    public class GetChildArticlesByIdSpecification : Specification<Article>
    {
        public GetChildArticlesByIdSpecification(List<string> ids)
        {
            Query
                .Where(x => ids.Contains(x.Id))
                .Include(x => x.Attachments)
                .Include(x => x.Comments);
        }
    }

    public class GetArticleByProjectIdSpecification : Specification<Article>
    {
        public GetArticleByProjectIdSpecification(string projectId)
        {
            Query.Where(x => x.ProjectId == projectId);
        }
    }

    public class GetArticlesWithCommentsSpecification : Specification<Article>
    {
        public GetArticlesWithCommentsSpecification()
        {
            Query.Include(x => x.Comments).Include(x=>x.Attachments);
        }
    }
}
