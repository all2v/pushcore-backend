using Astrum.Account.Features.Profile;
using Astrum.Account.Services;
using Astrum.Debts.Application.Models.CreateModels;
using Astrum.Debts.Application.Models.UpdateModels;
using Astrum.Debts.Application.Models.ViewModels;
using Astrum.Debts.Domain.Aggregates;
using Astrum.Debts.Domain.Specifications;
using Astrum.Debts.DomainServices.Repositories;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using MassTransit.NewIdProviders;

namespace Astrum.Debts.Application.Services
{
    //TODO: refactor DRY
    public class DebtsService : IDebtsService
    {
        private readonly IDebtsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserProfileService _userProfileServices;

        public DebtsService(IDebtsRepository repository, IMapper mapper, IUserProfileService userProfileService)
        {
            _repository = repository;
            _mapper = mapper;
            _userProfileServices = userProfileService;
        }


        public async Task<Result<List<DebtView>>> GetDebts(CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtsSpec();
            var debts = await _repository.ListAsync(spec, cancellationToken);
            var debtsView = _mapper.Map<List<DebtView>>(debts);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            foreach (var debt in debtsView) 
                SetDebtorAndBorrower(users, debt);
            return Result.Success(debtsView);
        }

        public async Task<Result<DebtView>> GetDebtById(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtByIdSpec(id);
            var debt = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (debt == null)
            {
                return Result.NotFound("Задолженность не найдена.");
            }
            var debtView = _mapper.Map<DebtView>(debt);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            SetDebtorAndBorrower(users, debtView);
            return Result.Success(debtView);
        }

        public async Task<Result<List<DebtView>>> GetDebtsByDebtorId(Guid debtorId, CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtsByDebtorIdSpec(debtorId);
            var debts = await _repository.ListAsync(spec, cancellationToken);
            var debtsView = _mapper.Map<List<DebtView>>(debts);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            foreach (var debt in debtsView) 
                SetDebtorAndBorrower(users, debt);
            return Result.Success(debtsView);
        }

        public async Task<Result<List<DebtView>>> GetDebtsByBorrowerId(Guid borrowerId, CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtsByLenderIdSpec(borrowerId);
            var debts = await _repository.ListAsync(spec, cancellationToken);
            var debtsView = _mapper.Map<List<DebtView>>(debts);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            foreach (var debt in debtsView)
                SetDebtorAndBorrower(users, debt);
            return Result.Success(debtsView);
        }

        public async Task<Result<List<DebtView>>> GetDebtsByStatus(StatusDebt status, CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtsByStatus(status);
            var debts = await _repository.ListAsync(spec, cancellationToken);
            var debtsView = _mapper.Map<List<DebtView>>(debts);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            foreach (var debt in debtsView)
                SetDebtorAndBorrower(users, debt);
            return Result.Success(debtsView);
        }

        public async Task<Result<DebtView>> CreateDebt(DebtCreateRequest debtCreate, CancellationToken cancellationToken = default)
        {
            var debt = _mapper.Map<Debt>(debtCreate);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            if (GetUser(users, debt.DebtorId) == null)
            {
                return Result.NotFound("Должник не найден.");
            }
            if(GetUser(users, debt.BorrowerId) == null)
            {
                return Result.NotFound("Кредитор не найден.");
            }
            debt = await _repository.AddAsync(debt, cancellationToken);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создании долга.");
            }
            var debtView = _mapper.Map<DebtView>(debt);
            SetDebtorAndBorrower(users, debtView);
            return Result.Success(debtView);
        }

        public async Task<Result<DebtView>> UpdateDebt(Guid id, DebtUpdateRequest debt, CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtByIdSpec(id);
            var debtDb = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            if (debtDb == null)
            {
                return Result.NotFound("Задолженность не найдена.");
            }
            if (GetUser(users, debt.DebtorId) == null)
            {
                return Result.NotFound("Должник не найден.");
            }
            else if (GetUser(users, debt.BorrowerId) == null)
            {
                return Result.NotFound("Кредитор не найден.");
            }
            _mapper.Map(debt, debtDb);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении долга.");
            }
            var debtView = _mapper.Map<DebtView>(debtDb);
            SetDebtorAndBorrower(users, debtView);
            return Result.Success(debtView);
        }

        public async Task<Result<DebtView>> DeleteDebt(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetDebtByIdSpec(id);
            var debt = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (debt == null)
            {
                return Result.NotFound("Задолженность не найдена.");
            }
            try
            {
                await _repository.DeleteAsync(debt, cancellationToken);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении задолженности.");
            }
            var debtView = _mapper.Map<DebtView>(debt);
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            SetDebtorAndBorrower(users, debtView);
            return Result.Success(debtView);
        }

        public async Task<Result<List<UserDebt>>> GetUsers(CancellationToken cancellationToken = default)
        {
            var users = await _userProfileServices.GetAllUsersProfilesSummariesAsync();
            var usersDebt = _mapper.Map<List<UserDebt>>(users.Data);
            return Result.Success(usersDebt);
        }

        private void SetDebtorAndBorrower(List<UserProfileSummary> users, DebtView debt)
        {
            debt.Debtor = GetUser(users, debt.DebtorId);
            debt.Borrower = GetUser(users, debt.BorrowerId);
        }

        private UserDebt? GetUser(Result<List<UserProfileSummary>> users, Guid userId)
        {
            if (users.Data.FirstOrDefault(user => user.UserId == userId) != null)
            {
                var user = _mapper.Map<UserDebt>(users.Data.FirstOrDefault(user => user.UserId == userId));
                return user;
            }

            return null;
        }
    }
}
