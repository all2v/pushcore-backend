using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Telegram.Domain.Aggregates
{
    public class TelegramChat : AggregateRootBase<Guid>
    {
        public string? Name { get; set; }
        public int ChatId { get; set; }
    }
}
