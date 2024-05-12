using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Repositories;
using Astrum.ITDictionary.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.ITDictionary.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, ICategoryRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<TermsCategoryView>> GetByIdAsync(Guid id)
    {
        var spec = new GetCategoryByIdSpec(id);
        var category = await _repository.FirstOrDefaultAsync(spec);
        if (category == null)
            return Result.NotFound("Категория не найдена.");

        var result = _mapper.Map<TermsCategoryView>(category);
        return Result.Success(result);
    }

    public async Task<Result<List<TermsCategoryView>>> GetAllAsync()
    {
        var spec = new GetCategorySpec();
        var categories = await _repository.ListAsync(spec);
        var result = _mapper.Map<List<TermsCategoryView>>(categories);
        return Result.Success(result);
    }

    public async Task<Result<List<TermsCategoryView>>> SearchAsync(string substring)
    {
        var spec = new GetCategoriesBySubstringSpec(substring);
        var categories = await _repository.ListAsync(spec);
        if (categories.Count == 0)
            return Result.NotFound("Категории не найдены.");

        var result = _mapper.Map<List<TermsCategoryView>>(categories);
        return Result.Success(result);
    }

    public async Task<Result<TermsCategoryView>> CreateAsync(CategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        await _repository.AddAsync(category);

        try
        {
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при создании категории.");
        }

        var result = _mapper.Map<TermsCategoryView>(category);
        return Result.Success(result);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var spec = new GetCategoryByIdSpec(id);
        var category = await _repository.FirstOrDefaultAsync(spec);
        if (category == null)
            return Result.NotFound("Категория не найдена.");

        try
        {
            await _repository.DeleteAsync(category);
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при удалении категории.");
        }

        return Result.Success();
    }
}