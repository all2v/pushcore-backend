using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Market.Application.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Profile.Commands
{
    public class MoneyChangeCommand : CommandResult<TransactionResponse>
    {
        public MoneyChangeCommand(Guid id, int moneyChange, bool spendMoney)
        {
            Id = id;
            MoneyChange = moneyChange;
            SpendMoney = spendMoney;
        }

        public Guid Id { get; set; }
        public bool SpendMoney { get; set; }
        public int MoneyChange { get; set; }
    }
}
