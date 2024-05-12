using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Projects.ViewModels.Views
{
    public class MemberView
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsManager { get; set; }
        public string Role { get; set; }
    }
}
