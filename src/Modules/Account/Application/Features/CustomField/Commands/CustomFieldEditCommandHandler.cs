using Astrum.Account.Features.CustomField.Commands;
using Astrum.Account.Features.CustomField;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.Account.Services;

namespace Astrum.Account.Features.CustomField.Commands
{
    public class CustomFieldEditCommandHandler : CommandResultHandler<CustomFieldEditCommand, CustomFieldResponse>
    {
        private readonly ICustomFieldService _customFieldService;

        public CustomFieldEditCommandHandler(ICustomFieldService customFieldService)
        {
            _customFieldService = customFieldService;
        }
        public override async Task<Result<CustomFieldResponse>> Handle(CustomFieldEditCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _customFieldService.EditCustomFieldAsync(command);
            return response;
        }
    }
}
