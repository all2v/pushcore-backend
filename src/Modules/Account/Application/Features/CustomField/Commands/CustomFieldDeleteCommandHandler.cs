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
    public sealed class CustomFieldDeleteCommandHandler : CommandResultHandler<CustomFieldDeleteCommand, CustomFieldResponse>
    {
        private readonly ICustomFieldService _customFieldService;

        public CustomFieldDeleteCommandHandler(ICustomFieldService customFieldService) 
        { 
            _customFieldService = customFieldService;
        }

        public override async Task<Result<CustomFieldResponse>> Handle(CustomFieldDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _customFieldService.DeleteCustomFieldAsync(command.Id);
            return response;
        }
    }
}
