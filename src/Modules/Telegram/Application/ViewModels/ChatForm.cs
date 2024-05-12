using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Telegram.Application.ViewModels
{
    public class ChatForm
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int ChatId { get; set; }
    }
}
