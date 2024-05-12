using Astrum.Account.Features.Profile.Commands;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public interface IContactsService
{
    public Task<Result<ApplicationUser>> UpdateAsync(ApplicationUser user, EditContactsCommand contacts);
}