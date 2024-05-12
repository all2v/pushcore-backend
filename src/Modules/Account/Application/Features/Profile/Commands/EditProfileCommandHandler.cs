using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Features.Commands;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Application.Features.Profile.Commands
{
    public class EditProfileCommandHandler : CommandResultHandler<EditProfileCommand>
    {
        private readonly IUserProfileRepository _profileRepository;

        public EditProfileCommandHandler(IUserProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public override async Task<Result> Handle(EditProfileCommand command, CancellationToken cancellationToken = default)
        {
            var specification = new GetUserProfileByUserIdSpec(command.UserId);
            var profile = await _profileRepository.FirstOrDefaultAsync(specification);
            if (profile == null)
                throw new Exception("User profile not found");

            profile.Name = command.FirstName ?? "";
            profile.Surname = command.LastName ?? "";
            profile.Patronymic = command.Patronymic ?? "";
            profile.Address = command.Address;
            profile.BirthDate = command.BirthDate;
            profile.PositionId = command.PositionId;

            await _profileRepository.UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
