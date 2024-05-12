using Astrum.Logging.Entities;
using Microsoft.Extensions.Logging;

namespace Astrum.Logging.ViewModels.Filters
{
    public class LogFilter
    {
        public LogLevel? LogLevel { get; set; }
        public DateTime? BeginPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public string? StatusCode { get; set; }
        public TypeRequest? TypeRequest { get; set; }
        public ModuleAstrum? Module { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }
        public string? BodyRequest { get; set; }
        public string? RequestResponse { get; set; }
    }
}
