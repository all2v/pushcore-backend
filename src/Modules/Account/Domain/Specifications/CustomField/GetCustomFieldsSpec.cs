using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Astrum.Account.Specifications.CustomField
{
    public class GetCustomFieldsSpec : Specification<Aggregates.CustomField>
    {
        public GetCustomFieldsSpec() 
        { 
        }
    }

    public class GetCustomFieldByIdSpec : GetCustomFieldsSpec
    {
        public GetCustomFieldByIdSpec(Guid Id) 
        { 
            Query
                .Where(customField => customField.Id == Id);
        }
    }
}
