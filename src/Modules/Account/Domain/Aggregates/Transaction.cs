using Astrum.Account.Aggregates;
using Astrum.Market.Domain.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Domain.Aggregates
{
    public class Transaction : AggregateRootBase<Guid>
    {
        public int Sum { get; set; }
        public Guid UserId { get; set; }
        public Guid? OwnerId { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
    }
}
