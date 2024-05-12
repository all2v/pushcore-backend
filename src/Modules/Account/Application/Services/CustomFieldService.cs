using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.Account.Features.CustomField;
using Astrum.Account.Features.CustomField.Commands;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.CustomField;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Account.Services
{
    public class CustomFieldService : ICustomFieldService
    {
        private readonly ICustomFieldRepository _customFieldRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public CustomFieldService(ICustomFieldRepository customFieldRepository, IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _customFieldRepository = customFieldRepository;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<Result<CustomFieldResponse>> CreateCustomFieldAsync(CustomFieldCreateCommand command)
        {
            var getUserProfileByUserIdSpec = new GetUserProfileByUserIdSpec(command.UserId);
            var profile = await _userProfileRepository.FirstOrDefaultAsync(getUserProfileByUserIdSpec);

            var customField = new CustomField()
            {
                Name = command.Name,
                Value = command.Value,
                UserProfileId = profile.Id
            };

            await _customFieldRepository.AddAsync(customField);
            try
            {
                await _customFieldRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создании кастомного поля.");
            }

            var response = _mapper.Map<CustomFieldResponse>(customField);
            return Result.Success(response);
        }
        public async Task<Result<CustomFieldResponse>> EditCustomFieldAsync(CustomFieldEditCommand command)
        {
            var spec = new GetCustomFieldByIdSpec(command.Id);
            var customField = await _customFieldRepository.FirstOrDefaultAsync(spec);
            if (customField == null)
            {
                return Result.NotFound("Кастомное поле не найдено");
            }

            customField.Value = command.Value;
            customField.Name = command.Name;

            try
            {
                await _customFieldRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении кастомного поля.");
            }

            var response = _mapper.Map<CustomFieldResponse>(customField);
            return Result.Success(response);
        }

        public async Task<Result<CustomFieldResponse>> DeleteCustomFieldAsync(Guid customFieldId)
        {
            var getCustomFieldByIdSpec = new GetCustomFieldByIdSpec(customFieldId);
            var customField = await _customFieldRepository.FirstOrDefaultAsync(getCustomFieldByIdSpec);

            if (customField == null)
                return Result.NotFound("Кастомное поле не найдено.");

            try
            {
                await _customFieldRepository.DeleteAsync(customField);
                await _customFieldRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении кастомного поля.");
            }

            var response = _mapper.Map<CustomFieldResponse>(customField);
            return Result.Success(response);
        }
    }
}
