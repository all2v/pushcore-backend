using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;
//using Astrum.Storage.Aggregates;

namespace Astrum.News.Aggregates
{
    public class PostFileAttachment : AggregateRootBase<Guid>
    {
        public PostFileAttachment() { }

        public PostFileAttachment(Guid id)
        {
            Id = id;
        }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid FileId { get; set; }
    }
}
