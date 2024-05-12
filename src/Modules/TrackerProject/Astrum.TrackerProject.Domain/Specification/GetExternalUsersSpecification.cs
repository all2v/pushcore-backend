using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.TrackerProject.Domain.Aggregates;

namespace Astrum.TrackerProject.Domain.Specification
{
    public class GetExternalUsersSpecification : Specification<ExternalUser>
    {
        public GetExternalUsersSpecification(ICollection<string> ids)
        {
            Query
                .Where(user => ids.Contains(user.Id));
        }
    }

    public class GetExternalUserSpecification : Specification<ExternalUser>
    {
        public GetExternalUserSpecification(string id)
        {
            Query
                .Where(user => id == user.Id);
        }
    }

    public class GetExternalUserByUsernameSpecification : Specification<ExternalUser>
    {
        public GetExternalUserByUsernameSpecification(string username)
        {
            Query
                .Where(user => user.UserName == username);
        }
    }

    public class GetOrderedExternalUsersSpecification : Specification<ExternalUser>
    {
        public GetOrderedExternalUsersSpecification(string username = null, string email = null)
        {
            Query
                .Where(x => username == null || x.UserName.ToLower().Contains(username.ToLower()))
                .Where(x => email == null || x.Email.ToLower().Contains(email.ToLower()))
                .OrderBy(x => x.UserName);
        }
    }

    public class GetExistedExternalUserSpecification : Specification<ExternalUser>
    {
        public GetExistedExternalUserSpecification(bool existed = false)
        {
            Query
                .Where(user => !existed || user.UserName != null);
        }
    }
}
