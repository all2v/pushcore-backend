using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Inventory.Application.Models.ViewModels
{
    public class InventoryItemsPaginationView
    {
        public List<InventoryItemView>? InventoryItems { get; set; }
        public int Index { get; set; }
        public bool NextExist { get; set; }
    }
}
