using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Astrum.Storage.Application.ViewModels
{
    public class MinimizedFileForm
    {
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public IFormFile FileBytes { get; set; }
    }
}
