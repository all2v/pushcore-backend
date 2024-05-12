using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.CustomField.Commands
{
    public class CustomFieldEditCommand : CommandResult<CustomFieldResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
    }
}
