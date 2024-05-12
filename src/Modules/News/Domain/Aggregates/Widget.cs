using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.News.Aggregates
{
    public class Widget : AggregateRootBase<Guid>
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public Guid? PictureId { get; set; }
    }
}
