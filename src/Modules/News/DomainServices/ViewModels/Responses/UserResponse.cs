using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.News.DomainServices.ViewModels.Responces
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string NameWithSurname { get; set; }
    }
}
