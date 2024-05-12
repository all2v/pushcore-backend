using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;

namespace Astrum.Account.Admin.ViewModels
{
    public class ChangeMoneyForm
    {
        [Required]
        public UserProfileSummary User;
        [Required]
        public int? MoneyChange;
    }
}
