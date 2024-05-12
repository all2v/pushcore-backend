using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.Market.Domain.Enums;

namespace Astrum.Market.Application.ViewModels
{
    public class TransactionRequest
    {
        public TransactionRequest(int sum, Guid id, TransactionType type, string? description = null, Guid? ownerId = null)
        {
            Sum = sum;
            UserId = id;
            Type = type;
            Description = description;
            OwnerId = ownerId;
        }

        public int Sum { get; set; }
        public Guid UserId { get; set; }
        public Guid? OwnerId { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
    }

    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public int Sum { get; set; }
        public Guid UserId { get; set; }
        public UserProfileSummary User { get; set; }
        public Guid? OwnerId { get; set; }
        public UserProfileSummary Owner { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
