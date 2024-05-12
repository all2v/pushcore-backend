using Astrum.Account.Aggregates;
using Astrum.Account.Application.Repositories;
using Astrum.Account.Domain.Aggregates;
using Astrum.Account.Domain.Specifications;
using Astrum.Account.Features.Profile;
using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Managers;
using Astrum.Identity.Repositories;
using Astrum.Market.Application.ViewModels;
using Astrum.Market.Domain.Enums;
using Astrum.SharedLib.Application.Extensions;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sakura.AspNetCore;

namespace Astrum.Account.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserProfileService _profileService;

        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository, 
            IUserProfileService profileService)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _profileService = profileService;
        }

        public async Task<Result<IPagedList<TransactionResponse>>> GetTransactions(int page = 1,
            int pageSize = 10, Guid? userId = null)
        {
            var users = await _profileService.GetAllUsersProfilesSummariesAsync();
            UserProfileSummary? user = null;
            if (userId != null)
            {
                user = users.Data.FirstOrDefault(x => x.UserId == userId);
                if (user == null) 
                    return Result.NotFound("Пользователь не найден");
            }
            var spec = new GetTransactionSpecification(userId);
            var transactions = await _transactionRepository.PagedListAsync(page, pageSize, spec);
            var response = transactions.ToMappedPagedList<Transaction, TransactionResponse>(_mapper, page, pageSize);
            foreach (var transaction in response)
            {
                if (user != null)
                    transaction.User = user;
                else
                {
                    var summary = users.Data.FirstOrDefault(x => x.UserId == transaction.UserId);
                    if(summary == null)
                        return Result.NotFound("Пользователь не найден");
                    transaction.User = summary;
                }
                var owner = users.Data.FirstOrDefault(x => x.UserId == transaction.OwnerId);
                if (transaction.OwnerId != null && owner == null)
                    return Result.NotFound("Пользователь не найден");
                transaction.Owner = owner;
            }

            return Result.Success(response);
        }

        public async Task<Result<int>> GetUserSum(Guid userId)
        {
            var spec = new GetTransactionSpecification(userId);
            var transactions = await _transactionRepository.ListAsync(spec);
            var sum = transactions.Sum(x => x.Sum);
            return Result.Success(sum);
        }

        public async Task<Result<TransactionResponse>> AddTransaction(TransactionRequest request)
        {
            var userResult = await _profileService.GetUserProfileAsync(request.UserId);
            if(userResult.Failed)
                return Result<TransactionResponse>.NotFound(userResult.MessageWithErrors);

            if (request.Sum < 0)
            {
                var balance = await GetUserSum(request.UserId);
                if (balance < request.Sum)
                    return Result.Error("У пользователя недостаточно денег для оплаты");
            }

            var transaction = _mapper.Map<Transaction>(request);
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.UnitOfWork.SaveChangesAsync();
            var result = _mapper.Map<TransactionResponse>(transaction);
            return Result.Success(result);
        }
    }
}
