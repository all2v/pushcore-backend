using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Application.Services;
using Astrum.Account.Services;
using Astrum.Market.Application.ViewModels;
using Astrum.Market.Domain.Enums;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Profile.Commands
{
    public class MoneyChangeCommandHandler : CommandResultHandler<MoneyChangeCommand, TransactionResponse>
    {
        private readonly ITransactionService _transactionService;

        public MoneyChangeCommandHandler(ITransactionService transactionService) : base()
        {
            _transactionService = transactionService;
        }

        public override async Task<Result<TransactionResponse>> Handle(MoneyChangeCommand command, CancellationToken cancellationToken = default)
        {
            var moneyChange = command.SpendMoney ? -command.MoneyChange : command.MoneyChange;
            var description = command.SpendMoney ? $"Списание {moneyChange} валюты у пользователя" : "Возвращение средств";
            var request = new TransactionRequest(moneyChange, command.Id, TransactionType.Market, description);
            var response = await _transactionService.AddTransaction(request);
            return response;
        }
    }
}
