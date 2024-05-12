using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.News.DomainServices.ViewModels.Requests
{
    public class LikeDeleteRequest
    {
        public Guid PostId { get; set; }
        public Guid From { get; set; }
    }
}
