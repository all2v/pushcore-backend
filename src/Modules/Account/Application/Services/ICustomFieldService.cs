using Astrum.Account.Features.CustomField;
using Astrum.Account.Features.CustomField.Commands;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services
{
    public interface ICustomFieldService
    {
        public Task<Result<CustomFieldResponse>> CreateCustomFieldAsync(CustomFieldCreateCommand command);
        public Task<Result<CustomFieldResponse>> EditCustomFieldAsync(CustomFieldEditCommand command);
        public Task<Result<CustomFieldResponse>> DeleteCustomFieldAsync(Guid customFieldId);
    }
}
