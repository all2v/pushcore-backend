using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Logging.ViewModels
{
    public class LogAdminView : CommonLogForm
    {
        public string Description { get; set; }
        public string BodyRequest { get; set; }
        public string RequestResponse { get; set; }
        public ResultStatus Status { get; set; }

    }
}
