using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.Achievements;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Account.Application.Features.Achievement.Queries.GetAchievementsList
{
    public class GetAchievementsListQueryHandler : QueryResultHandler<GetAchievementsListQuery, List<AchievementResponse>>
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;

        public GetAchievementsListQueryHandler(IAchievementRepository achievementRepository, IMapper mapper, IFileStorage fileStorage)
        {
            _achievementRepository = achievementRepository;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        public override async Task<Result<List<AchievementResponse>>> 
            Handle(GetAchievementsListQuery query, CancellationToken cancellationToken = default)
        {
            var spec = new GetAchievementsSpec();
            var achievements = await _achievementRepository.ListAsync(spec, cancellationToken);
            var response = _mapper.Map<List<AchievementResponse>>(achievements);

            foreach(var achievement in response)
            {
                if (achievement.IconId.HasValue)
                    achievement.IconUrl = await _fileStorage.GetFileUrl(achievement.IconId.Value);
            }

            return response;
        }
    }
}
