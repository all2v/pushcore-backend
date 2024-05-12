using Astrum.Market.Application.ViewModels;
using Sakura.AspNetCore;

namespace Astrum.Account.Application.Services
{
    public interface ITransactionService
    {
        Task<SharedLib.Common.Results.Result<IPagedList<TransactionResponse>>> GetTransactions(int page = 1, int pageSize = 10, Guid? userId = null);
        Task<SharedLib.Common.Results.Result<int>> GetUserSum(Guid userId);
        Task<SharedLib.Common.Results.Result<TransactionResponse>> AddTransaction(TransactionRequest request);
    }
}
