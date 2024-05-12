using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Repositories;
using Astrum.ITDictionary.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.ITDictionary.Services;

public class TermService : ITermService
{
    private readonly ITermRepository _repository;
    private readonly ICategoryService _categoryService;

    private readonly IMapper _mapper;

    public TermService(ITermRepository repository, ICategoryService categoryService, IMapper mapper)
    {
        _repository = repository;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<Result<TermView>> GetByIdAsync(Guid id)
    {
        var spec = new GetTermByIdSpec(id);
        var term = await _repository.FirstOrDefaultAsync(spec);
        if (term == null)
            return Result.NotFound("Термин не найден");

        var result = _mapper.Map<TermView>(term);
        return Result.Success(result);
    }

    public async Task<Result<List<TermView>>> GetAllAsync()
    {
        var spec = new GetTermSpec();
        var terms = await _repository.ListAsync(spec);
        var result = _mapper.Map<List<TermView>>(terms);
        return Result.Success(result);
    }

    public async Task<Result<List<TermView>>> GetByCategoryIdAsync(Guid categoryId)
    {
        var spec = new GetTermsByCategoryIdSpec(categoryId);
        var terms = await _repository.ListAsync(spec);
        if (terms.Count == 0)
            return Result.NotFound("Термины не найдены.");

        var result = _mapper.Map<List<TermView>>(terms);
        return Result.Success(result);
    }

    public async Task<Result<List<TermView>>> SearchAsync(string substring)
    {
        var spec = new GetTermsBySubstringSpec(substring);
        var terms = await _repository.ListAsync(spec);
        if (terms.Count == 0)
            return Result.NotFound("Термины не найдены.");

        var result = _mapper.Map<List<TermView>>(terms);
        return Result.Success(result);
    }

    public async Task<Result<TermView>> CreateAsync(TermRequest request)
    {
        var term = _mapper.Map<Term>(request);
        await _repository.AddAsync(term);

        try
        {
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при создании термина.");
        }

        var result = _mapper.Map<TermView>(term);
        return Result.Success(result);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var spec = new GetTermByIdSpec(id);
        var term = await _repository.FirstOrDefaultAsync(spec);
        if (term == null)
            return Result.NotFound("Термин не найден");

        try
        {
            await _repository.DeleteAsync(term);
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при удалении термина");
        }
        
        return Result.Success();
    }
}