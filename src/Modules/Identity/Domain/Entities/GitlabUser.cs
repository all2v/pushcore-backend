using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Identity.Domain.Entities
{
    public class GitlabUser : IEntity<long>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
