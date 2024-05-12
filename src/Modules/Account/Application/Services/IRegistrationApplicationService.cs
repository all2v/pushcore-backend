using Astrum.Account.Features.Registration;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationCreate;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdate;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public interface IRegistrationApplicationService
{
    public Task<RegistrationApplicationResponse> CreateAsync(RegistrationApplicationCreateCommand command);
    public Task<RegistrationApplicationResponse> DeleteAsync(Guid id);
    public Task<List<RegistrationApplicationResponse>> GetAllAsync();
    public Task<RegistrationApplicationResponse> GetAsync(Guid id);
    public Task<RegistrationApplicationResponse> UpdateAsync(RegistrationApplicationUpdateCommand command);
    public Task<Result> ApproveRegistrationApplicationAsync(Guid applicationId);
    public Task<Result> DeclineRegistrationApplicationAsync(Guid applicationId);
    public Task<RegistrationApplicationResponse> UpdateApplicationStatusAsync(
        RegistrationApplicationUpdateStatusCommand command);
}