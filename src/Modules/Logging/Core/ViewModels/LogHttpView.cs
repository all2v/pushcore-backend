using Astrum.Logging.Entities;

namespace Astrum.Logging.ViewModels
{
    public class LogHttpView : CommonLogForm
    {
        public string StatusCode { get; set; }
        public TypeRequest TypeRequest { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string BodyRequest { get; set; }
        public string RequestResponse { get; set; }
    }
}
