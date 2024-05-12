using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Account.Domain.Aggregates;

namespace Astrum.Account.Domain.Specifications
{
    public class GetTransactionSpecification : Specification<Transaction>
    {
        public GetTransactionSpecification(Guid? userId = null)
        {
            Query
                .Where(x => userId == null || x.UserId == userId)
                .OrderByDescending(x => x.DateCreated);
        }
    }
}
