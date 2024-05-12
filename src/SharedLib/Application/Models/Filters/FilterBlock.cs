using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.SharedLib.Application.Models.Filters
{
    public class FilterBlock
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public List<FilterItem> FilterItems { get; set; }
    }
}
