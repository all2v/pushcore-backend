using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile.Queries;
using Astrum.Account.Features.Profile;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Account.Services;
using MassTransit;
using AutoMapper;
using Astrum.IdentityServer.Domain.ViewModels;
using NetBox.Extensions;
using Astrum.News.DomainServices.ViewModels.Responces;
using Astrum.Account.Application.Features.Profile.Queries;
using Astrum.News.Services;

namespace Astrum.News.Application.Features.Handlers
{
    public class GetUserPostsQueryHandler : QueryResultHandler<GetUserPostsQuery, List<PostResponse>>
    {
        private readonly IUserProfileService userProfileService;
        private readonly IMapper mapper;
        private readonly INewsService newsService;

        public GetUserPostsQueryHandler(IUserProfileService userProfileService, IMapper mapper, INewsService newsService)
        {
            this.userProfileService = userProfileService;
            this.mapper = mapper;
            this.newsService = newsService;
        }

        public override async Task<SharedLib.Common.Results.Result<List<PostResponse>>> Handle(GetUserPostsQuery query, 
            CancellationToken cancellationToken = default)
        {
            var userResult = await userProfileService.GetUserByUsername(query.Username);
            if (userResult.Failed)
                return Result.Error(userResult.MessageWithErrors);

            var posts = await newsService.GetPostsByUser(userResult.Data.Id, query.StartIndex, query.Count);
            
            var result = mapper.Map<List<PostResponse>>(posts.Data);
            return Result.Success(result);
        }
    }
}
