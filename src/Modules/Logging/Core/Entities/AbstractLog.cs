using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Astrum.Logging.Entities
{
    public class AbstractLog : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
        public LogLevel LogLevel { get; set; }
        public ModuleAstrum Module { get; set; }

    }
}
