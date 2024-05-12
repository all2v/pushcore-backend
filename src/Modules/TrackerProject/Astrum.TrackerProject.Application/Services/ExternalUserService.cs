using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.Account.Services;
using Astrum.SharedLib.Application.Extensions;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;
using Astrum.TrackerProject.Domain.Aggregates;
using Astrum.TrackerProject.Domain.Repositories;
using Astrum.TrackerProject.Domain.Specification;
using AutoMapper;
using Minio.DataModel;
using Sakura.AspNetCore;
using static MassTransit.ValidationResultExtensions;
using Result = Astrum.SharedLib.Common.Results.Result;

namespace Astrum.TrackerProject.Application.Services
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly ITrackerProjectRepository<ExternalUser> _repository;
        private readonly IUserProfileService _profileService;
        private readonly IMapper _mapper;

        public ExternalUserService(ITrackerProjectRepository<ExternalUser> repository, IUserProfileService profileService, 
            IMapper mapper)
        {
            _repository = repository;
            _profileService = profileService;
            _mapper = mapper;
        }

        public async Task<Result<IPagedList<ExternalUserForm>>> GetUserProfilesAsync(int pageIndex = 1, int pageSize = 10,
            ExternalUserFilter filter = null)
        {
            var spec = new GetOrderedExternalUsersSpecification(filter?.UserName, filter?.Email);
            var users = await _repository.PagedListAsync(pageIndex, pageSize, spec);
            //var summaries = (await _profileService.GetAllUsersProfilesSummariesAsync()).Data;
            var userForms = users.ToMappedPagedList<ExternalUser, ExternalUserForm>(_mapper, pageIndex, pageSize);
            return Result.Success(userForms);
        }

        public async Task<Result<List<ExternalUserForm>>> GetAllUserProfilesAsync(bool existed = false)
        {
            var spec = new GetExistedExternalUserSpecification(existed);
            var users = await _repository.ListAsync(spec);
            var summaries = (await _profileService.GetAllUsersProfilesSummariesAsync()).Data;
            var userForms = _mapper.Map<List<ExternalUserForm>>(users);
            //foreach (var userForm in userForms)
            //{
            //    userForm.UserProfileSummary = summaries
            //        .Where(summary => summary != null)
            //        .FirstOrDefault(summary => summary.Username == userForm.UserName);
            //}
            foreach (var userForm in userForms)
            {
                userForm.UserProfileSummary = summaries.FirstOrDefault(x => x.Username == userForm.UserName);
            }
            return Result.Success(userForms);
        }

        public async Task<ExternalUserForm> UpdateUser(ExternalUserForm userForm)
        {
            var spec = new GetExternalUserSpecification(userForm.Id);
            var user = await _repository.FirstOrDefaultAsync(spec);
            user.UserName = userForm.UserName;
            await _repository.UpdateAsync(user);
            await _repository.UnitOfWork.SaveChangesAsync();
            return userForm;
        }
    }
}
