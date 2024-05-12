using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Astrum.Storage.ViewModels;

namespace Astrum.Account.Application.Features.Profile.Commands
{
    public class ChangeAvatarCommandHandler : CommandHandler<ChangeAvatarCommand, Result<ChangeAvatarResponse>>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IFileStorage _fileStorage;
        private readonly IApplicationUserRepository _userRepository;
        private const string StorageAccountBucketName = "account";

        public ChangeAvatarCommandHandler(IUserProfileRepository profileRepository, 
            IFileStorage fileStorage, IApplicationUserRepository userRepository)
        {
            _profileRepository = profileRepository;
            _fileStorage = fileStorage;
            _userRepository = userRepository;
        }

        public override async Task<Result<ChangeAvatarResponse>> Handle(ChangeAvatarCommand command, CancellationToken cancellationToken = default)
        {
            if (command.Username == null || command.AvatarImage == null)
                return Result.Error("Данные пусты.");

            var specification = new GetUserByUsernameSpec(command.Username);
            var user = await _userRepository.FirstOrDefaultAsync(specification);
            if (user == null)
                return Result.NotFound("Пользователь не найден.");

            var profileSpec = new GetUserProfileByUserIdSpec(user.Id);
            var profile = await _profileRepository.FirstOrDefaultAsync(profileSpec);
            if (profile == null)
                return Result.NotFound("Профиль пользователя не найден.");

            byte[] imageBytes = null;
            using (var binaryReader = new BinaryReader(command.AvatarImage.OpenReadStream()))
            {
                imageBytes = binaryReader.ReadBytes((int)command.AvatarImage.Length);
            }

            var avatar = new FileForm()
            {
                ContentType = command.AvatarImage.ContentType,
                FileName = command.AvatarImage.Name,
                FileBytes = imageBytes
            };
            var uploadResult = await _fileStorage.UploadFile(avatar, cancellationToken);
            profile.AvatarImageId = uploadResult.UploadedFileId;
            try
            {
                await _profileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                var url = await _fileStorage.GetFileUrl(uploadResult.UploadedFileId.Value);
                var response = new ChangeAvatarResponse { FileUrl = url };
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при изменении аватарки пользователя.");
            }
        }
    }
}
