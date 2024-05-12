using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Application.Services;
using Astrum.Account.Features.Profile.Queries;
using Astrum.Account.Features.Profile;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Account.Services;
using AutoMapper;
using Astrum.Identity.Managers;
using Astrum.SharedLib.Persistence.Helpers;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetBasicUserInfoByIdQueryHandler : QueryResultHandler<GetBasicUserInfoByIdQuery, BasicUserInfoResponse>
    {
        private readonly IUserProfileService userProfileService;
        private readonly IMapper mapper;
        private readonly ApplicationUserManager userManager;
        private readonly ITransactionService transactionService;

        public GetBasicUserInfoByIdQueryHandler(IUserProfileService userProfileService, IMapper mapper, 
            ApplicationUserManager userManager, ITransactionService transactionService)
        {
            this.userProfileService = userProfileService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.transactionService = transactionService;
        }

        public override async Task<Result<BasicUserInfoResponse>> Handle(GetBasicUserInfoByIdQuery query, CancellationToken cancellationToken = default)
        {
            var userProfile = await userProfileService.GetUserProfileAsync(query.Id);
            var result = mapper.Map<BasicUserInfoResponse>(userProfile.Data);
            var user = await userManager.FindByIdAsync(query.Id.ToString());
            if (user == null)
                return Result.NotFound("Пользователь не найден.");
            result.UserName = user.UserName;
            result.Money = await transactionService.GetUserSum(result.UserId);
            var strRoles = await userManager.GetRolesAsync(user);
            var enumRoles = RolesHelper.MapToEnumRoles(strRoles);
            result.Roles = enumRoles.Select(r => (int)r).ToList();
            return Result.Success(result);
        }
    }
}
