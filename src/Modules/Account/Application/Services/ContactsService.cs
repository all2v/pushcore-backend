using Astrum.Account.Features.Profile.Commands;
using Astrum.Identity.Models;
using Astrum.Identity.Repositories;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public class ContactsService : IContactsService
{
    private readonly IApplicationUserRepository _userRepository;

    public ContactsService(IApplicationUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    #region IContactsService Members

    public async Task<Result<ApplicationUser>> UpdateAsync(ApplicationUser user, EditContactsCommand contacts)
    {
        user.PhoneNumber = contacts?.PhoneNumber ?? user.PhoneNumber;
        user.Email = contacts?.Email ?? user.Email;
        try
        {
            await _userRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении контактов.");
        }
        return Result.Success(user);
    }

    #endregion
}