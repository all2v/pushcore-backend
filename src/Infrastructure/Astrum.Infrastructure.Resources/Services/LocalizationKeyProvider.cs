using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Domain.Enums;

namespace Astrum.Infrastructure.Resources.Services;

public class LocalizationKeyProvider : ILocalizationKeyProvider
{
    #region ILocalizationKeyProvider Members

    public string GetRoleLocalizationKey(RolesEnum role)
    {
        return role switch
        {
            RolesEnum.Trainee => ResourceKeys.Roles_Trainee,
            RolesEnum.Employee => ResourceKeys.Roles_Employee,
            RolesEnum.Manager => ResourceKeys.Roles_Manager,
            RolesEnum.Admin => ResourceKeys.Roles_Admin,
            RolesEnum.SuperAdmin => ResourceKeys.Roles_SuperAdmin,
            _ => role.ToString()
        };
    }

    #endregion
}