using Astrum.Projects.Aggregates;
using Astrum.Projects.Repositories;
using Astrum.Projects.Specifications.Customer;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Projects.Services;

public class CustomFieldService : ICustomFieldService
{
    private readonly ICustomFieldRepository _customFieldRepository;
    private readonly IMapper _mapper;

    public CustomFieldService(ICustomFieldRepository customFieldRepository, IMapper mapper)
    {
        _customFieldRepository = customFieldRepository;
        _mapper = mapper;
    }

    #region ICustomFieldService Members

    public async Task<Result<CustomFieldView>> Create(CustomFieldRequest customFieldView)
    {
        var newCustomField = _mapper.Map<CustomField>(customFieldView);
        await _customFieldRepository.AddAsync(newCustomField);
        try
        {
            await _customFieldRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании кастомного поля.");
        }
        return Result.Success(_mapper.Map<CustomFieldView>(newCustomField));
    }

    public async Task<Result<CustomFieldView>> Delete(Guid id)
    {
        var spec = new GetCustomFieldByIdSpec(id);
        var customField = await _customFieldRepository.FirstOrDefaultAsync(spec);
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
        return Result.Success(_mapper.Map<CustomFieldView>(customField));
    }

    #endregion
}