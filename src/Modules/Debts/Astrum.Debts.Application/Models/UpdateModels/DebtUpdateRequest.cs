using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Debts.Domain.Aggregates;

namespace Astrum.Debts.Application.Models.UpdateModels
{
    public class DebtUpdateRequest
    {
        public string Description { get; set; }
        public DateTimeOffset DateDebt { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModifed { get; set; }
        public Guid BorrowerId { get; set; }
        public Guid DebtorId { get; set; }
        public int SumDebt { get; set; }
        public StatusDebt Status { get; set; }
    }
}
