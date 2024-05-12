using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementAssign;
using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.Achievements;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Account.Application.Features.Achievement.Commands.AchievementRemove
{
    public class
        AchievementRemoveCommandHandler : CommandResultHandler<AchievementRemoveCommand, UserAchievementResponse>
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserProfileRepository _profileRepository;

        public AchievementRemoveCommandHandler(IAchievementRepository achievementRepository,
            IUserProfileRepository profileRepository)
        {
            _achievementRepository = achievementRepository;
            _profileRepository = profileRepository;
        }

        public override async Task<Result<UserAchievementResponse>> Handle(AchievementRemoveCommand command,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            /*var userSpec = new GetUserByUsernameSpec(command.Username);
            var user = await _applicationUserRepository.FirstOrDefaultAsync(userSpec, cancellationToken);
            if (user == null)
                return Result.NotFound("User not found");

            var profileSpec = new GetUserProfileByUserIdSpec(user.Id);
            var profile = await _profileRepository.FirstOrDefaultAsync(profileSpec, cancellationToken);
            if (profile == null)
                return Result.NotFound("Profile not found");

            var achievementSpec = new GetAchievementByIdSpec(command.AchievementId);
            var achievement = await _achievementRepository.FirstOrDefaultAsync(achievementSpec, cancellationToken);
            if (achievement == null)
                return Result.NotFound("Achievement not found");

            var userDoesntHaveAchievement = !profile.Achievements.Contains(achievement);
            if (userDoesntHaveAchievement)
                return Result.Error("User does not have this achievement");

            profile.Achievements.Remove(achievement);
            await _profileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();*/
        }
    }
}
