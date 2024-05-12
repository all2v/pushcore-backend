using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.CustomField.Commands
{
    public class CustomFieldDeleteCommand : CommandResult<CustomFieldResponse>
    {
        public CustomFieldDeleteCommand([FromRoute] Guid Id) 
        { 
            this.Id = Id;
        }

        public Guid Id { get; set; }
    }
}
