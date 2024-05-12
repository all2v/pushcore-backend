using Ardalis.Specification;
using Astrum.Logging.Entities;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Logging.Repositories
{
    public class LogAdminRepository : EFRepository<LogAdmin, Guid, LogsDbContext>, ILogAdminRepository
    {
        public LogAdminRepository(LogsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator) { }
    }
}
