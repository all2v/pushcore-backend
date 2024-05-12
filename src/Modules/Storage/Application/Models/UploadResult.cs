using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Storage.Application.Models
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public Guid? UploadedFileId { get; set; }
    }
}
