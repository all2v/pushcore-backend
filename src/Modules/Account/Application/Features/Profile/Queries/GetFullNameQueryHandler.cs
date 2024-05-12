using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Services;
using Astrum.Identity.Managers;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Persistence.Helpers;
using AutoMapper;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetFullNameQueryHandler : QueryResultHandler<GetFullNameQuery, FullNameResponce>
    {
        private readonly IUserProfileService userProfileService;
        private readonly IMapper mapper;
        private readonly ApplicationUserManager userManager;

        public GetFullNameQueryHandler(IUserProfileService userProfileService, IMapper mapper,
            ApplicationUserManager userManager)
        {
            this.userProfileService = userProfileService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public override async Task<Result<FullNameResponce>> Handle(GetFullNameQuery query, CancellationToken cancellationToken = default)
        {
            var userProfile = await userProfileService.GetUserProfileByUsernameAsync(query.Username);
            var result = mapper.Map<FullNameResponce>(userProfile.Data);
            return Result.Success(result);
        }
    }
}
