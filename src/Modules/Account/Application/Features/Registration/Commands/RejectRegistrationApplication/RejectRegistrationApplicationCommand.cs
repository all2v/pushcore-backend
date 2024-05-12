using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Registration.Commands.RejectRegistrationApplication
{
    public class RejectRegistrationApplicationCommand : CommandResult
    {
        public Guid ApplicationId { get; set; }
    }
}
