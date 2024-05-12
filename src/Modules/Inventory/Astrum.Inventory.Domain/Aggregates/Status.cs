using System.ComponentModel.DataAnnotations;

namespace Astrum.Inventory.Domain.Aggregates
{
    public enum Status
    {
        [Display(Name = "В использовании")]
        InUsing,
        [Display(Name = "На складе")]
        InWarehouse,
        [Display(Name = "Пропало")]
        Miss
    }
}