using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.IdentityServer.Domain.ViewModels
{
    public class UserViewModel : UserBase
    {
        public Guid Id { get; set; }
    }
}
