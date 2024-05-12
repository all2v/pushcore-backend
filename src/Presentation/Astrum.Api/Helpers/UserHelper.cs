using System;
using System.Collections.Generic;
using System.Linq;
using Astrum.Identity.Contracts;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Astrum.Api.Helpers;

public class UserHelper
{
    private readonly ILocalizationKeyProvider _localizationKeyProvider;
    private readonly ILocalizationService _localizer;
    private readonly IAuthenticatedUserService _userService;

    public UserHelper(IAuthenticatedUserService userService, ILocalizationService localizer,
        ILocalizationKeyProvider localizationKeyProvider)
    {
        _localizer = localizer;
        _localizationKeyProvider = localizationKeyProvider;
        _userService = userService;
    }

    public IEnumerable<SelectListItem> GetAvailableRoles()
    {
        throw new NotImplementedException();
        // var currentUserHighestRole = _userService.Roles.Max();
        //
        // return Enum.GetValues(typeof(RolesEnum))
        //     .Cast<RolesEnum>()
        //     .Where(e => e <= currentUserHighestRole)
        //     .Select(e => new SelectListItem
        //     {
        //         Value = ((int)e).ToString(),
        //         Text = _localizer[_localizationKeyProvider.GetRoleLocalizationKey(e)]
        //     });
    }
}