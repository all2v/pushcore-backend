using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models.Article
{
    public class TrackerArticleComment
    {
        public string Id { get; set; }
        public SynchronisationUser Author { get; set; }
        public string Text { get; set; }
        public List<TrackerAttachment> Attachments { get; set; }

    }
}
