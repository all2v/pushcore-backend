using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories
{
    public class CustomFieldRepository : EFRepository<CustomField, Guid, AccountDbContext>,
        ICustomFieldRepository
    {
        public CustomFieldRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : 
            base(context, specificationEvaluator)
        {

        }
    }
}
