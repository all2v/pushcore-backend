using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Enums;

namespace Astrum.Account.Application.ViewModels
{
    public class UsersFilter
    {
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public RolesEnum? Role { get; set; }
        public Guid? PositionId { get; set; }
    }
}
