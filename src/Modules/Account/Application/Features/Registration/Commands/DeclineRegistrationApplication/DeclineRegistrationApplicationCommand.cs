using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Registration.Commands.DeclineRegistrationApplication
{
    public class DeclineRegistrationApplicationCommand : CommandResult
    {
        public Guid ApplicationId { get; set; }

        public DeclineRegistrationApplicationCommand(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
