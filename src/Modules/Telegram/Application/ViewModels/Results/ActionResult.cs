using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Telegram.Application.ViewModels.Results
{
    public class ActionResult
    {
        public bool Succeed { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
