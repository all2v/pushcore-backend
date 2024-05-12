using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Inventory.Domain.Aggregates;

namespace Astrum.Inventory.Application.Models.ViewModels
{
    public class FilterInfo
    {
        public List<TemplateView>? Templates { get; set; }
        public List<Status>? Statuses { get; set; }
    }
}
