using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.CustomField.Commands
{
    public class CustomFieldCreateCommandHandler : CommandResultHandler<CustomFieldCreateCommand, CustomFieldResponse>
    {
        private readonly ICustomFieldService _customFieldService;

        public CustomFieldCreateCommandHandler(ICustomFieldService customFieldService) 
        { 
            _customFieldService = customFieldService;
        }
        public override async Task<Result<CustomFieldResponse>> Handle(CustomFieldCreateCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _customFieldService.CreateCustomFieldAsync(command);
            return response;
        }
    }
}
