using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.Account.Application.Repositories;
using Astrum.Account.Domain.Aggregates;
using Astrum.Account.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Persistence.Repositories
{
    public class PositionRepository : EFRepository<Position, Guid, AccountDbContext>,
    IPositionRepository
    {
        public PositionRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
            base(context, specificationEvaluator)
        {
        }
    }
}
