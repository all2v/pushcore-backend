using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Identity.Application.ViewModels
{
    public class TokenOperationResult
    {
        public bool Successful { get; set; }
        public string AccessToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
