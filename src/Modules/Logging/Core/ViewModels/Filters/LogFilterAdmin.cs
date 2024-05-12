using Astrum.Logging.Entities;
using Astrum.SharedLib.Common.Results;
using Microsoft.Extensions.Logging;

namespace Astrum.Logging.ViewModels.Filters
{
    public class LogFilterAdmin
    {
        public ModuleAstrum? Module { get; set; }
        public LogLevel? LogLevel { get; set; }
        public DateTime? BeginPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public string? Description { get; set; }
        public ResultStatus? Status { get; set; }
        public string? BodyRequest { get; set; }
        public string? RequestResponse { get; set; }
    }
}
