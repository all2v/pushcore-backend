using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.ITDictionary.Services;

public interface ITermConstructorService
{
    public Task<List<Term>> GetSelectedTerms(Guid userId);
    
    public Task<Result<List<TermView>>> GetSelectedTermsResult(Guid userId);

    public Task<List<UserTerm>> GetUserTerms(Guid userId);

    public Task<Result> UpdateUserTerms(UserTermsRequest request);
}