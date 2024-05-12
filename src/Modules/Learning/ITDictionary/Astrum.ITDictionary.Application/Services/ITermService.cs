using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.ITDictionary.Services;

public interface ITermService
{
    public Task<Result<TermView>> GetByIdAsync(Guid id);

    public Task<Result<List<TermView>>> GetAllAsync();

    public Task<Result<List<TermView>>> GetByCategoryIdAsync(Guid categoryId);

    public Task<Result<List<TermView>>> SearchAsync(string substring);

    public Task<Result<TermView>> CreateAsync(TermRequest request);

    public Task<Result> DeleteAsync(Guid id);
}