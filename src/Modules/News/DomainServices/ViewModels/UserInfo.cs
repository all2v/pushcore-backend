using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.News.ViewModels
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameWithSurname
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
        public string Position { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
