using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Market.Domain.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Начисление")]
        Accrual,
        [Display(Name = "Списание")]
        WriteOff,
        [Display(Name = "Магазин")]
        Market
    }
}
