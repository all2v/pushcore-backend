using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;
using Sakura.AspNetCore;

namespace Astrum.TrackerProject.Application.Services
{
    public interface IExternalUserService
    {
        Task<Result<IPagedList<ExternalUserForm>>> GetUserProfilesAsync(int pageIndex = 1, int pageSize = 10,
            ExternalUserFilter filter = null);
        Task<ExternalUserForm> UpdateUser(ExternalUserForm userForm);
        Task<Result<List<ExternalUserForm>>> GetAllUserProfilesAsync(bool existed = false);
    }
}
