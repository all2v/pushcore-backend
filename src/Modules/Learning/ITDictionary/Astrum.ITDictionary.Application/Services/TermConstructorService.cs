using Astrum.Identity.Models;
using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Repositories;
using Astrum.ITDictionary.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Astrum.ITDictionary.Services;

public class TermConstructorService : ITermConstructorService
{
    private readonly IUserTermRepository _repository;
    private readonly ITermRepository _termRepository;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public TermConstructorService(IUserTermRepository repository, UserManager<ApplicationUser> userManager,
        IMapper mapper, ITermRepository termRepository)
    {
        _repository = repository;
        _userManager = userManager;
        _mapper = mapper;
        _termRepository = termRepository;
    }

    public async Task<List<UserTerm>> GetUserTerms(Guid userId)
    {
        var spec = new GetUserTermByUserIdSpec(userId);
        var userTerms = await _repository.ListAsync(spec);
        return userTerms;
    }

    public async Task<List<Term>> GetSelectedTerms(Guid userId)
    {
        var spec = new GetSelectedUserTermsSpec(userId);
        var terms = (await _repository.ListAsync(spec))
        .Select(e => e.Term)
        .ToList();
        return terms;
    }

    public async Task<Result<List<TermView>>> GetSelectedTermsResult(Guid userId)
    {
        var terms = await GetSelectedTerms(userId);

        return Result.Success(_mapper.Map<List<TermView>>(terms));
    }

    public async Task<Result> UpdateUserTerms(UserTermsRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result.NotFound("Пользователь не найден.");

        var userTerms = await GetUserTerms(request.UserId);

        var repositoryTerms = await _termRepository.ListAsync(new GetTermsByIdsSpec(request.TermIds));
        var toAdd = repositoryTerms
            .Where(t => userTerms.All(ut => ut.TermId != t.Id))
            .Select(t => t.Id);
        
        var addResult = await Add(toAdd, request.UserId);
        var updateResult = await Update(userTerms, request.TermIds);
        if (addResult.Failed) return Result.Error(addResult.MessageWithErrors);
        if (updateResult.Failed) return Result.Error(updateResult.MessageWithErrors);
        return Result.Success();
    }

    private async Task<Result> Update(List<UserTerm> userTerms, ICollection<Guid> selected)
    {
        foreach (var userTerm in userTerms)
        {
            userTerm.IsSelected = selected.Contains(userTerm.TermId);
        }

        await _repository.UpdateRangeAsync(userTerms);
        try
        {
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при изменении терминов пользователя.");
        }

        return Result.Success();
    }

    private async Task<Result> Add(IEnumerable<Guid> termIds, Guid userId)
    {
        var userTerms = termIds
            .Select(term => new UserTerm
            {
                TermId = term,
                UserId = userId,
                IsSelected = true
            });
        await _repository.AddRangeAsync(userTerms);
        try
        {
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при добавлении терминов пользователю.");
        }

        return Result.Success();
    }
}