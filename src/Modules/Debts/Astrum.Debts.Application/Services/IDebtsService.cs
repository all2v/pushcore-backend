using Astrum.Debts.Application.Models.CreateModels;
using Astrum.Debts.Application.Models.UpdateModels;
using Astrum.Debts.Application.Models.ViewModels;
using Astrum.Debts.Domain.Aggregates;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Debts.Application.Services
{
    public interface IDebtsService
    {
        Task<Result<List<DebtView>>> GetDebts(CancellationToken cancellationToken = default);
        Task<Result<List<DebtView>>> GetDebtsByBorrowerId(Guid lenderId, CancellationToken cancellationToken = default);
        Task<Result<List<DebtView>>> GetDebtsByDebtorId(Guid debtorId, CancellationToken cancellationToken = default);
        Task<Result<List<DebtView>>> GetDebtsByStatus(StatusDebt status, CancellationToken cancellationToken = default);
        Task<Result<DebtView>> GetDebtById(Guid id, CancellationToken cancellationToken = default);
        Task<Result<DebtView>> CreateDebt(DebtCreateRequest debtCreate, CancellationToken cancellationToken = default);
        Task<Result<DebtView>> UpdateDebt(Guid id, DebtUpdateRequest debtUpdate, CancellationToken cancellationToken = default);
        Task<Result<DebtView>> DeleteDebt(Guid id, CancellationToken cancellationToken = default);
        Task<Result<List<UserDebt>>> GetUsers(CancellationToken cancellationToken = default);

    }
}
