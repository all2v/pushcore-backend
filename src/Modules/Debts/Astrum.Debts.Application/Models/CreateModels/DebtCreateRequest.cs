using Astrum.Debts.Domain.Aggregates;

namespace Astrum.Debts.Application.Models.CreateModels
{
    public class DebtCreateRequest
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
