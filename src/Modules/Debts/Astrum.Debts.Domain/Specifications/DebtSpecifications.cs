using Ardalis.Specification;
using Astrum.Debts.Domain.Aggregates;

namespace Astrum.Debts.Domain.Specifications
{
    public class GetDebtsSpec : Specification<Debt> { }
    public class GetDebtByIdSpec : GetDebtsSpec
    {
        public GetDebtByIdSpec(Guid id)
        {
            Query.Where(debt => debt.Id == id);
        }
    }
    public class GetDebtsByLenderIdSpec : GetDebtsSpec
    {
        public GetDebtsByLenderIdSpec(Guid lenderId)
        {
            Query.Where(debt => debt.BorrowerId == lenderId);
        }
    }

    public class GetDebtsByDebtorIdSpec : GetDebtsSpec
    {
        public GetDebtsByDebtorIdSpec(Guid debtorId)
        {
            Query.Where(debt => debt.DebtorId == debtorId);
        }
    }

    public class GetDebtsByStatus : GetDebtsSpec
    {
        public GetDebtsByStatus(StatusDebt status)
        {
            Query.Where(debt => debt.Status == status);
        }
    }
}
