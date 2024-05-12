using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.CustomField;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.CustomField.Commands
{
    public class CustomFieldCreateCommand : CommandResult<CustomFieldResponse>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
    }
}
