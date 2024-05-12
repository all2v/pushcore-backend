using Astrum.Appeal.Aggregates;
using Astrum.Appeal.Application.Services;
using Astrum.Appeal.Repositories;
using Astrum.Appeal.Specifications;
using Astrum.Appeal.ViewModels;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Appeal.Services;

public class AppealCategoryService : IAppealCategoryService
{
    private readonly IAppealCategoryRepository _appealCategoryRepository;
    private readonly IMapper _mapper;

    public AppealCategoryService(IAppealCategoryRepository appealCategoryRepository, IMapper mapper)
    {
        _appealCategoryRepository = appealCategoryRepository;
        _mapper = mapper;
    }

    #region IAppealCategoryService Members

    public async Task<SharedLib.Common.Results.Result<List<AppealCategoryForm>>> GetAppealCategories()
    {
        var categories = await _appealCategoryRepository.ListAsync();
        return Result.Success(_mapper.Map<List<AppealCategoryForm>>(categories));
    }

    public async Task<SharedLib.Common.Results.Result<AppealCategoryForm>> CreateCategory(string name)
    {
        var category = new AppealCategory(name);
        category = await _appealCategoryRepository.AddAsync(category);
        try
        {
            await _appealCategoryRepository.UnitOfWork.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании категории.");
        }
        var appealCategoryForm = _mapper.Map<AppealCategoryForm>(category);
        return Result.Success(appealCategoryForm);
    }

    public async Task<SharedLib.Common.Results.Result<AppealCategoryForm>> UpdateCategory(Guid id, string name)
    {
        var category = await _appealCategoryRepository.GetByIdAsync(id);
        if (category == null)
            return Result.NotFound("Категория заявки не найдена.");
        category.Category = name;
        try
        {
            await _appealCategoryRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании категории.");
        }
        var appealCategoryForm = _mapper.Map<AppealCategoryForm>(category);
        return Result.Success(appealCategoryForm);
    }

    public async Task<SharedLib.Common.Results.Result<AppealCategoryForm>> DeleteCategory(Guid id)
    {
        var spec = new GetCategoryById(id);
        var category = await _appealCategoryRepository.FirstOrDefaultAsync(spec);

        if (category == null)
            return Result.NotFound("Категория заявки не найдена.");
        try
        {
            await _appealCategoryRepository.DeleteAsync(category);
            await _appealCategoryRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении категории.");
        }
        return Result.Success(_mapper.Map<AppealCategoryForm>(category));
    }

    public async Task<bool> CategoryAlreadyExists(string categoryName, CancellationToken cancellationToken = default)
    {
        var categories = await _appealCategoryRepository.ListAsync();
        return categories.Any(category => category.Category.ToLower().Trim() == categoryName.ToLower().Trim());
    }

    #endregion
}