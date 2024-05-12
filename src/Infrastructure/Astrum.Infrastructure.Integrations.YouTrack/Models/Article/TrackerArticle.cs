using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models.Article
{
    public class TrackerArticle
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public SynchronisationUser Reporter { get; set; }
        public TrackerProject Project { get; set; }
        public TrackerArticle ParentArticle { get; set; }
        public List<TrackerArticle> ChildArticles { get; set; }
        public List<TrackerArticleComment> Comments { get; set; }
        public List<TrackerAttachment> Attachments { get; set; }
        public bool IsNew { get; set; }

    }
}
