using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Account.Aggregates;

namespace Astrum.Account.Specifications.RegistrationApplication
{
    public class GetRegistrationApplicationsSpec : Specification<Aggregates.RegistrationApplication>
    {
        public GetRegistrationApplicationsSpec() 
        {
        }
    }

    public class GetRegistrationApplicationByIdSpec : GetRegistrationApplicationsSpec
    {
        public GetRegistrationApplicationByIdSpec(Guid applicationId) 
        { 
            Query
                .Where(application => application.Id == applicationId);
        }
    }
}
