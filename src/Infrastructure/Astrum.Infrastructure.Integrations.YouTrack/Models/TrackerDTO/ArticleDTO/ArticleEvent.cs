using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.ArticleDTO
{
    public class ArticleEvent
    {
        public bool IsNew { get; set; }
        public Project Project { get; set; }
        public Comment NewComment { get; set; }
        public Article Article { get; set; }
        public User CreatedBy { get; set; }
        public User UpdatedBy { get; set; }
    }
}
