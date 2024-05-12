using Ardalis.Specification;
using Astrum.Logging.Entities;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Logging.Repositories
{
    public class LogHttpRepository : EFRepository<LogHttp, Guid, LogsDbContext>, ILogHttpRepository
    {
        public LogHttpRepository(LogsDbContext context, ISpecificationEvaluator specificationEvaluator = null) : base(context, specificationEvaluator) { }
    }
}
