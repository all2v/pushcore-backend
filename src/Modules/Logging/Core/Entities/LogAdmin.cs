using Astrum.SharedLib.Common.Results;

namespace Astrum.Logging.Entities
{
    public class LogAdmin : AbstractLog
    {
        public string Description { get; set; }
        public string BodyRequest { get; set; }
        public string RequestResponse { get; set; }
        public ResultStatus Status { get; set; }
    }
}
