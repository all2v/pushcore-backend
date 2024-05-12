using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Models;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Identity.Domain.Entities
{
    public class GitLabUsersMappings : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; }
        public long GitlabUserId { get; set; }
        public GitlabUser GitlabUser { get; set; }
    }
}
