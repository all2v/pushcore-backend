using Astrum.Logging.Entities;
using Microsoft.Extensions.Logging;

namespace Astrum.Logging.ViewModels
{
    public abstract class CommonLogForm
    {
        public LogLevel LogLevel { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public ModuleAstrum Module { get; set; }

    }
}
