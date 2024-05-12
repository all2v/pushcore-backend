using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Module.Project.Application.ViewModels.DTO
{
    public class RemoveMembersDto
    {
        public Guid ProjectId { get; set; }
        public List<Guid> UsersId { get; set; }
    }
}
