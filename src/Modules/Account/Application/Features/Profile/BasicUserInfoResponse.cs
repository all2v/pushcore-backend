using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Account.Application.Features.Profile
{
    public class BasicUserInfoResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public int Money { get; set; }
        public string? AvatarUrl { get; set; }
        public List<int> Roles { get; set; }
    }
}
