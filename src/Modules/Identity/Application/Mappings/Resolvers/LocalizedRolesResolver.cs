using Astrum.Identity.Models;
using Astrum.Identity.ReadModels;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Domain.Enums;
using AutoMapper;
using static System.Enum;

namespace Astrum.Identity.Mappings.Resolvers;

public class LocalizedRolesResolver : IValueResolver<ApplicationUser, UserReadModel, IReadOnlyCollection<string>>
{
    private readonly ILocalizationKeyProvider _localizationKeyProvider;
    private readonly ILocalizationService _localizationService;

    public LocalizedRolesResolver(ILocalizationService localizationService,
        ILocalizationKeyProvider localizationKeyProvider)
    {
        _localizationService = localizationService;
        _localizationKeyProvider = localizationKeyProvider;
    }

    #region IValueResolver<ApplicationUser,UserReadModel,IReadOnlyCollection<string>> Members

    public IReadOnlyCollection<string> Resolve(ApplicationUser source, UserReadModel destination,
        IReadOnlyCollection<string> destMember, ResolutionContext context)
    {
        var result = new List<string>();
        foreach (var role in source.Roles)
        {
            var tryParse = TryParse<RolesEnum>(role.Role.Name, out var rolesEnum);
            if (!tryParse) throw new Exception(nameof(RolesEnum) + "doesn't contain " + role.Role.Name);
            result.Add(_localizationService[_localizationKeyProvider.GetRoleLocalizationKey(rolesEnum)]);
        }

        return result;
    }

    #endregion
}