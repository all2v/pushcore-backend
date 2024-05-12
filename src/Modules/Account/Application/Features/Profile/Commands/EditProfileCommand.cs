using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Profile.Commands
{
    public class EditProfileCommand : CommandResult
    {
        public Guid UserId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Address { get; set; }
        public Guid? PositionId { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
