using System.ComponentModel.DataAnnotations;

namespace Astrum.Market.ViewModels;

public enum OrderStatus
{
    [Display(Name = "Заказан")]
    Ordered,
    [Display(Name = "Принят в работу")]
    Accepted,
    [Display(Name = "Отменён")]
    Cancelled,
    [Display(Name = "Завершён")]
    Completed
}