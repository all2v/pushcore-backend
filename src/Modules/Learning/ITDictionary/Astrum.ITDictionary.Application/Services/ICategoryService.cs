using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.ITDictionary.Services;

public interface ICategoryService
{
    public Task<Result<TermsCategoryView>> GetByIdAsync(Guid id);

    public Task<Result<List<TermsCategoryView>>> GetAllAsync();

    public Task<Result<List<TermsCategoryView>>> SearchAsync(string substring);

    public Task<Result<TermsCategoryView>> CreateAsync(CategoryRequest request);

    public Task<Result> DeleteAsync(Guid id);
}