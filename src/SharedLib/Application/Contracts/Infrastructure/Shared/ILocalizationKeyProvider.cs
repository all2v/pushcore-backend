using Astrum.SharedLib.Domain.Enums;

namespace Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;

/// <summary>
///     Provides various localization keys for resolving to localized resources
/// </summary>
public interface ILocalizationKeyProvider
{
    string GetRoleLocalizationKey(RolesEnum role);
}