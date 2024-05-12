using Astrum.SharedLib.Domain.Entities;
using JetBrains.Annotations;

namespace Astrum.Debts.Domain.Aggregates
{
    public class Debt : AggregateRootBase<Guid>
    {
        public string Description { get; set; }
        public DateTimeOffset DateDebt { get; set; }
        public Guid BorrowerId { get; set; }
        public Guid DebtorId { get; set; }
        public int SumDebt { get; set; }
        public StatusDebt Status { get; set; }
    }
}
